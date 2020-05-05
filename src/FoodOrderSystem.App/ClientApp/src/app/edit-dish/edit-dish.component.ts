import { Component, OnInit, Input } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DishModel } from '../dish-category/dish.model';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';

@Component({
  selector: 'app-edit-dish',
  templateUrl: './edit-dish.component.html',
  styleUrls: ['./edit-dish.component.css']
})
export class EditDishComponent implements OnInit {
  @Input() public restaurantId: string;
  @Input() public dishCategoryId: string;
  @Input() public dish: DishModel;
  @BlockUI() blockUI: NgBlockUI;

  isNew: boolean;
  editDishForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private restaurantAdminService: RestaurantRestAdminService
  ) {
  }

  ngOnInit() {
    this.isNew = this.dish === undefined || this.dish.id === undefined;

    this.editDishForm = this.formBuilder.group({
      name: this.dish !== undefined ? this.dish.name : '',
      description: this.dish !== undefined ? this.dish.description : '',
      productInfo: this.dish !== undefined ? this.dish.productInfo : '',
    });
  }

  onSubmit(data) {
    console.log("Data: ", data);

    this.blockUI.start("Verarbeite Daten...");

    if (this.dish === undefined)
      this.dish = new DishModel();
    this.dish.name = data.name;
    this.dish.description = data.description;
    this.dish.productInfo = data.productInfo;

    let subscription = this.restaurantAdminService.addOrChangeDishOfRestaurantAsync(this.restaurantId, this.dishCategoryId, this.dish)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.editDishForm.reset();
        this.activeModal.close(this.dish);
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
