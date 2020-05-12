import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { DishCategoryModel } from '../dish-category/dish-category.model';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-change-dish-category',
  templateUrl: './change-dish-category.component.html',
  styleUrls: ['./change-dish-category.component.css', '../../assets/css/admin-forms.min.css']
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
    private restaurantAdminService: RestaurantRestAdminService
  ) {
  }

  ngOnInit() {
    this.changeDishCategoryForm = this.formBuilder.group({
      name: this.dishCategory.name
    });
  }

  onSubmit(data) {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantAdminService.changeDishCategoryOfRestaurantAsync(this.restaurantId, this.dishCategory.id, data.name)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.changeDishCategoryForm.reset();
        this.activeModal.close(new DishCategoryModel({ id: this.dishCategory.id, name: data.name }));
      }, (status: number) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        if (status === 401)
          this.message = "Sie sind nicht angemdeldet.";
        else if (status === 403)
          this.message = "Sie sind nicht berechtigt, diese Aktion durchzuführen.";
        else
          this.message = "Ein unvorhergesehener Fehler ist aufgetreten. Bitte versuchen Sie es nochmals.";
      });
  }
}
