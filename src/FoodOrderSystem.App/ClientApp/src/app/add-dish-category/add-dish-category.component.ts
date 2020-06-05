import { Component, OnInit, Input } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';
import { DishCategoryModel } from '../dish-category/dish-category.model';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-add-dish-category',
  templateUrl: './add-dish-category.component.html',
  styleUrls: ['./add-dish-category.component.css', '../../assets/css/admin-forms.min.css']
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

  get f() { return this.addDishCategoryForm.controls; }

  onSubmit(data) {
    this.submitted = true;
    if (this.addDishCategoryForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantAdminService
      .addDishCategoryToRestaurantAsync(this.restaurantId, data.name, this.afterCategoryId)
      .subscribe((id) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.addDishCategoryForm.reset();
        this.activeModal.close(new DishCategoryModel({ id, name: data.name }));
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.addDishCategoryForm.reset();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
