import {Component, OnInit, Input} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpErrorResponse} from '@angular/common/http';

import {take} from 'rxjs/operators';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {RestaurantModel} from '../../../shared/models/restaurant.model';

import {HttpErrorHandlingService} from '../../../shared/services/http-error-handling.service';

import {RestaurantRestAdminService} from '../../services/restaurant-rest-admin.service';

@Component({
  selector: 'app-change-restaurant-name',
  templateUrl: './change-restaurant-name.component.html',
  styleUrls: [
    './change-restaurant-name.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class ChangeRestaurantNameComponent implements OnInit {
  @Input() public restaurant: RestaurantModel;
  @BlockUI() blockUI: NgBlockUI;

  changeRestaurantNameForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private restaurantAdminService: RestaurantRestAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
    this.changeRestaurantNameForm = this.formBuilder.group({
      name: [this.restaurant.name, Validators.required]
    });
  }

  get f() {
    return this.changeRestaurantNameForm.controls;
  }

  onSubmit(data) {
    if (this.changeRestaurantNameForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantAdminService.changeRestaurantNameAsync(this.restaurant.id, data.name)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.changeRestaurantNameForm.reset();
        this.activeModal.close(data.name);
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
