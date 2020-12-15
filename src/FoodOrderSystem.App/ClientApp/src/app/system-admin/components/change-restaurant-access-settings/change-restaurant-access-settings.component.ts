import {Component, Input, OnInit} from '@angular/core';
import {FormBuilder} from "@angular/forms";

import {Observable, of} from "rxjs";
import {concatMap, debounceTime, distinctUntilChanged, map, switchMap, tap} from "rxjs/operators";

import {NgbActiveModal, NgbTypeaheadSelectItemEvent} from "@ng-bootstrap/ng-bootstrap";

import {BlockUI, NgBlockUI} from "ng-block-ui";

import {RestaurantModel} from "../../../shared/models/restaurant.model";

import {HttpErrorHandlingService} from "../../../shared/services/http-error-handling.service";

import {CuisineAdminService} from "../../services/cuisine-admin.service";
import {RestaurantSysAdminService} from "../../services/restaurant-sys-admin.service";
import {UserModel} from "../../../shared/models/user.model";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-change-restaurant-access-settings',
  templateUrl: './change-restaurant-access-settings.component.html',
  styleUrls: [
    './change-restaurant-access-settings.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class ChangeRestaurantAccessSettingsComponent implements OnInit {
  @Input() public restaurant: RestaurantModel;
  @BlockUI() blockUI: NgBlockUI;

  administrators: UserModel[];
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
    this.administrators = Object.assign([], this.restaurant.administrators);
    if (this.administrators === undefined) {
      this.administrators = new Array<UserModel>();
    }
  }

  searchForUser = (text: Observable<string>) =>
    text.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => {
        if (term.length < 2) {
          return of([]);
        } else {
          return this.restaurantAdminService.searchForUsersAsync(term)
            .pipe(
              map(users => {
                if (users.length > 2) {
                  return [users[0], users[1]];
                } else {
                  return users;
                }
              })
            )
        }
      }),
    )

  formatUser(user: UserModel): string {
    return user.email;
  }

  onUserSelected(event: NgbTypeaheadSelectItemEvent, input: HTMLInputElement): void {
    const user: UserModel = event.item;

    event.preventDefault();
    input.value = '';

    const index = this.administrators.findIndex(en => en.id === user.id);
    if (index < 0) {
      this.administrators.push(user);
      this.sortAdministrators();
    }
  }

  onRemoveUser(user: UserModel): void {
    const index = this.administrators.findIndex(en => en.id === user.id);
    this.administrators.splice(index, 1);
  }

  sortAdministrators(): void {
    this.administrators.sort((a, b) => {
      if (a.email < b.email)
        return -1;
      else if (a.email > b.email)
        return 1;
      return 0;
    });
  }

  onSave(): void {
    this.blockUI.start('Verarbeite Daten...');

    let curObservable: Observable<boolean> = undefined;

    for (let index = this.administrators.length - 1; index >= 0; index--) {
      const administrator = this.administrators[index];

      const adminIndex = this.restaurant.administrators !== undefined
        ? this.restaurant.administrators.findIndex(en => en.id === administrator.id)
        : -1;

      if (adminIndex < 0) {
        const nextObservable = this.restaurantAdminService.addAdminToRestaurantAsync(this.restaurant.id, administrator.id)
          .pipe(
            tap(() => {
              this.restaurant.administrators.push(administrator);
            })
          );
        if (curObservable !== undefined) {
          curObservable = curObservable.pipe(concatMap(() => nextObservable));
        } else {
          curObservable = nextObservable;
        }
      }
    }

    if (this.restaurant.administrators !== undefined) {
      for (let index = this.restaurant.administrators.length - 1; index >= 0; index--) {
        const administrator = this.restaurant.administrators[index];

        const adminIndex = this.administrators.findIndex(en => en.id === administrator.id);
        if (adminIndex < 0) {
          const nextObservable = this.restaurantAdminService.removeAdminFromRestaurantAsync(this.restaurant.id, administrator.id)
            .pipe(
              tap(() => {
                const tempIndex = this.restaurant.administrators.findIndex(en => en.id === administrator.id);
                this.restaurant.administrators.splice(tempIndex, 1);
              })
            );
          if (curObservable !== undefined) {
            curObservable = curObservable.pipe(concatMap(() => nextObservable));
          } else {
            curObservable = nextObservable;
          }
        }
      }
    }

    if (curObservable === undefined) {
      curObservable = of(true);
    }

    curObservable?.subscribe(() => {
      this.blockUI.stop();
      this.message = undefined;
      this.activeModal.close('Close click');
    }, (response: HttpErrorResponse) => {
      this.blockUI.stop();
      this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

}
