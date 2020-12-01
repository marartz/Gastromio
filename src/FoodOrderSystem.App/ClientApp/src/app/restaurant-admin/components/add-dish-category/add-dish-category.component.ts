import {Component, OnInit, Input} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';
import {FormGroup, FormBuilder, Validators} from '@angular/forms';

import {take} from 'rxjs/operators';

import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {DishCategoryModel} from '../../../shared/models/dish-category.model';

import {HttpErrorHandlingService} from '../../../shared/services/http-error-handling.service';

import {RestaurantRestAdminService} from '../../services/restaurant-rest-admin.service';

@Component({
  selector: 'app-add-dish-category',
  templateUrl: './add-dish-category.component.html',
  styleUrls: [
    './add-dish-category.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class AddDishCategoryComponent implements OnInit {
  @Input() public restaurantId: string;
  @Input() public afterCategoryId: string;
  @BlockUI() blockUI: NgBlockUI;

  addDishCategoryForm: FormGroup;
  message: string;
  submitted = false;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private restaurantAdminService: RestaurantRestAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
    this.addDishCategoryForm = this.formBuilder.group({
      name: ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  get f() {
    return this.addDishCategoryForm.controls;
  }

  onSubmit(data) {
    this.submitted = true;
    if (this.addDishCategoryForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantAdminService.addDishCategoryToRestaurantAsync(this.restaurantId, data.name, this.afterCategoryId)
      .pipe(take(1))
      .subscribe((id) => {
        this.blockUI.stop();
        this.message = undefined;
        this.addDishCategoryForm.reset();
        this.activeModal.close(new DishCategoryModel({id, name: data.name}));
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
