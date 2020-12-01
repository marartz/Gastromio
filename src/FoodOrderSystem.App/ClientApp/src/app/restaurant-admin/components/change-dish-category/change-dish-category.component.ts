import {Component, OnInit, Input} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpErrorResponse} from '@angular/common/http';

import {take} from 'rxjs/operators';

import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {DishCategoryModel} from '../../../shared/models/dish-category.model';

import {HttpErrorHandlingService} from '../../../shared/services/http-error-handling.service';

import {RestaurantRestAdminService} from '../../services/restaurant-rest-admin.service';

@Component({
  selector: 'app-change-dish-category',
  templateUrl: './change-dish-category.component.html',
  styleUrls: [
    './change-dish-category.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class ChangeDishCategoryComponent implements OnInit {
  @Input() public restaurantId: string;
  @Input() public dishCategory: DishCategoryModel;
  @BlockUI() blockUI: NgBlockUI;

  changeDishCategoryForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private restaurantAdminService: RestaurantRestAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
    this.changeDishCategoryForm = this.formBuilder.group({
      name: [this.dishCategory.name, Validators.required]
    });
  }

  get f() {
    return this.changeDishCategoryForm.controls;
  }

  onSubmit(data) {
    if (this.changeDishCategoryForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantAdminService.changeDishCategoryOfRestaurantAsync(this.restaurantId, this.dishCategory.id, data.name)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.changeDishCategoryForm.reset();
        this.activeModal.close(new DishCategoryModel({id: this.dishCategory.id, name: data.name}));
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
