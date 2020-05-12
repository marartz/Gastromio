import { Component, OnInit, Input } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';
import { DishCategoryModel } from '../dish-category/dish-category.model';

@Component({
  selector: 'app-add-dish-category',
  templateUrl: './add-dish-category.component.html',
  styleUrls: ['./add-dish-category.component.css', '../../assets/css/admin-forms.min.css']
})
export class AddDishCategoryComponent implements OnInit {
  @Input() public restaurantId: string;
  @BlockUI() blockUI: NgBlockUI;

  addDishCategoryForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private restaurantAdminService: RestaurantRestAdminService
  ) {
    this.addDishCategoryForm = this.formBuilder.group({
      name: ''
    });
  }

  ngOnInit(): void {
  }

  onSubmit(data) {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantAdminService.addDishCategoryToRestaurantAsync(this.restaurantId, data.name)
      .subscribe((id) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.addDishCategoryForm.reset();
        this.activeModal.close(new DishCategoryModel({ id: id, name: data.name }));
      }, (status: number) => {
        subscription.unsubscribe();
        this.blockUI.stop();
          this.addDishCategoryForm.reset();
        if (status === 401)
          this.message = "Sie sind nicht angemdeldet.";
        else if (status === 403)
          this.message = "Sie sind nicht berechtigt, diese Aktion durchzuführen.";
        else
          this.message = "Ein unvorhergesehener Fehler ist aufgetreten. Bitte versuchen Sie es nochmals.";
      });
  }
}
