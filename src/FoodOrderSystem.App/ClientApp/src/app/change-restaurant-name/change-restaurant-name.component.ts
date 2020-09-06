import {Component, OnInit, Input} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {RestaurantRestAdminService} from '../restaurant-rest-admin/restaurant-rest-admin.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {HttpErrorResponse} from '@angular/common/http';
import {take} from 'rxjs/operators';

@Component({
  selector: 'app-change-restaurant-name',
  templateUrl: './change-restaurant-name.component.html',
  styleUrls: [
    './change-restaurant-name.component.css',
    '../../assets/css/frontend_v2.min.css',
    '../../assets/css/backend_v2.min.css',
    '../../assets/css/animations_v2.min.css'
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