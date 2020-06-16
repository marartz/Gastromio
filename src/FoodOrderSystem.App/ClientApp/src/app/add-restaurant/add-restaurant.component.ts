import {Component, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

import {RestaurantSysAdminService} from '../restaurant-sys-admin/restaurant-sys-admin.service';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {HttpErrorResponse} from '@angular/common/http';
import {take} from 'rxjs/operators';

@Component({
  selector: 'app-add-restaurant',
  templateUrl: './add-restaurant.component.html',
  styleUrls: ['./add-restaurant.component.css', '../../assets/css/frontend.min.css']
})
export class AddRestaurantComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;

  addRestaurantForm: FormGroup;
  message: string;
  submitted = false;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private restaurantAdminService: RestaurantSysAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
    this.addRestaurantForm = this.formBuilder.group({
      name: ['', Validators.required]
    });
  }

  ngOnInit() {
  }

  get f() {
    return this.addRestaurantForm.controls;
  }

  onSubmit(data) {
    this.submitted = true;
    if (this.addRestaurantForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantAdminService.addRestaurantAsync(data.name)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.addRestaurantForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.addRestaurantForm.reset();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
