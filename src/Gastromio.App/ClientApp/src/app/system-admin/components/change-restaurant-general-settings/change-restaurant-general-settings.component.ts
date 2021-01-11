import {Component, Input, OnInit} from '@angular/core';
import {HttpErrorResponse} from "@angular/common/http";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

import {BehaviorSubject, Observable, of} from "rxjs";
import {concatMap, tap} from "rxjs/operators";

import {NgbActiveModal} from "@ng-bootstrap/ng-bootstrap";

import {BlockUI, NgBlockUI} from "ng-block-ui";

import {RestaurantModel} from "../../../shared/models/restaurant.model";

import {HttpErrorHandlingService} from "../../../shared/services/http-error-handling.service";

import {CuisineAdminService} from "../../services/cuisine-admin.service";
import {RestaurantSysAdminService} from "../../services/restaurant-sys-admin.service";

@Component({
  selector: 'app-change-restaurant-general-settings',
  templateUrl: './change-restaurant-general-settings.component.html',
  styleUrls: [
    './change-restaurant-general-settings.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class ChangeRestaurantGeneralSettingsComponent implements OnInit {
  @Input() public restaurant: RestaurantModel;
  @BlockUI() blockUI: NgBlockUI;

  cuisineStatusArray$: BehaviorSubject<CuisineStatus[]> = new BehaviorSubject<CuisineStatus[]>(new Array<CuisineStatus>());

  changeSettingsForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private cuisineAdminService: CuisineAdminService,
    private restaurantAdminService: RestaurantSysAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit(): void {
    this.cuisineAdminService.getAllCuisinesAsync()
      .subscribe(cuisines => {
        cuisines.sort((a, b) => {
          if (a.name < b.name)
            return -1;
          else if (a.name > b.name)
            return 1;
          return 0;
        });

        const cuisineStatusArray = this.cuisineStatusArray$.value;
        for (let cuisine of cuisines) {
          const status = this.restaurant.cuisines.some(en => en.id === cuisine.id);
          cuisineStatusArray.push(new CuisineStatus({
            id: cuisine.id,
            name: cuisine.name,
            oldStatus: status,
            newStatus: status
          }));
        }

        this.cuisineStatusArray$.next(this.cuisineStatusArray$.value);
      });

    this.changeSettingsForm = this.formBuilder.group({
      name: [this.restaurant.name, Validators.required],
      alias: [this.restaurant.alias, Validators.required],
      importId: [this.restaurant.importId]
    });
  }

  get f() {
    return this.changeSettingsForm.controls;
  }

  toggleCuisine(cuisineId: string): void {
    const cuisineStatusArray = this.cuisineStatusArray$.value;
    const index = cuisineStatusArray.findIndex(en => en.id === cuisineId);
    const cuisineStatus = cuisineStatusArray[index];
    cuisineStatus.newStatus = !cuisineStatus.newStatus;
    this.cuisineStatusArray$.next(this.cuisineStatusArray$.value);
  }

  isAtLeastOneCuisineEnabled(): boolean {
    const cuisineStatusArray = this.cuisineStatusArray$.value;
    return cuisineStatusArray.some(en => en.newStatus);
  }

  onSubmit(data): void {
    if (!this.changeSettingsForm.valid) {
      return;
    }

    if (!this.isAtLeastOneCuisineEnabled()) {
      this.message = "Bitte wähle mindestens eine Cuisine aus."
      return;
    }

    this.blockUI.start('Verarbeite Daten...');

    let curObservable: Observable<boolean> = undefined;

    const cuisineStatusArray = this.cuisineStatusArray$.value;
    for (let index = cuisineStatusArray.length - 1; index >= 0; index--) {
      const cuisineStatus = cuisineStatusArray[index];

      if (cuisineStatus.oldStatus != cuisineStatus.newStatus) {
        let nextChangeCuisineObservable: Observable<boolean> = undefined;
        if (cuisineStatus.newStatus) {
          nextChangeCuisineObservable = this.restaurantAdminService.addCuisineToRestaurantAsync(this.restaurant.id, cuisineStatus.id);
        } else {
          nextChangeCuisineObservable = this.restaurantAdminService.removeCuisineFromRestaurantAsync(this.restaurant.id, cuisineStatus.id);
        }

        if (curObservable !== undefined) {
          curObservable = curObservable.pipe(concatMap(() => nextChangeCuisineObservable));
        } else {
          curObservable = nextChangeCuisineObservable;
        }
      }
    }

    if (this.restaurant.alias !== data.alias) {
      const nextObservable = this.restaurantAdminService.setRestaurantAliasAsync(this.restaurant.id, data.alias)
      if (curObservable !== undefined) {
        curObservable = curObservable.pipe(concatMap(() => nextObservable));
      } else {
        curObservable = nextObservable;
      }
    }

    if (this.restaurant.importId !== data.importId) {
      const nextObservable = this.restaurantAdminService.setRestaurantImportIdAsync(this.restaurant.id, data.importId)
      if (curObservable !== undefined) {
        curObservable = curObservable.pipe(concatMap(() => nextObservable));
      } else {
        curObservable = nextObservable;
      }
    }

    let observable: Observable<boolean>;

    if (this.restaurant.name !== data.name) {
      observable = this.restaurantAdminService.changeRestaurantNameAsync(this.restaurant.id, data.name)
        .pipe(
          tap(() => {
            this.changeSettingsForm.reset();
          }),
          concatMap(() => curObservable ?? of(true))
        )
    } else {
      observable = curObservable ?? of(true);
    }

    observable.subscribe(() => {
      this.blockUI.stop();
      this.message = undefined;
      this.activeModal.close('Close click');
    }, (response: HttpErrorResponse) => {
      this.blockUI.stop();
      this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

}


export class CuisineStatus {

  constructor(init?: Partial<CuisineStatus>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  id: string;
  name: string;
  oldStatus: boolean;
  newStatus: boolean;

}
