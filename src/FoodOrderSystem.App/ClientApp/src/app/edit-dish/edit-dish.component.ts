import { Component, OnInit, Input } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DishModel } from '../dish-category/dish.model';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';
import { DishVariantModel } from '../dish-category/dish-variant.model';
import { Guid } from "guid-typescript";

@Component({
  selector: 'app-edit-dish',
  templateUrl: './edit-dish.component.html',
  styleUrls: ['./edit-dish.component.css', '../../assets/css/admin-forms.min.css']
})
export class EditDishComponent implements OnInit {
  @Input() public restaurantId: string;
  @Input() public dishCategoryId: string;
  @Input() public dish: DishModel;
  @BlockUI() blockUI: NgBlockUI;

  isNew: boolean;
  editDishForm: FormGroup;
  message: string;

  price: number;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private restaurantAdminService: RestaurantRestAdminService
  ) {
  }

  ngOnInit() {
    if (this.dish === undefined) {
      this.dish = new DishModel();
      this.dish.variants = new Array<DishVariantModel>();
    }

    this.isNew = this.dish.id === undefined;

    this.editDishForm = this.formBuilder.group({
      name: this.dish !== undefined ? this.dish.name : '',
      description: this.dish !== undefined ? this.dish.description : '',
      productInfo: this.dish !== undefined ? this.dish.productInfo : '',
    });

    if (this.dish.variants.length === 0) {
      this.price = 0;
    } else if (this.dish.variants.length === 1) {
      this.price = this.dish.variants[0].price;
    }
  }

  onAddVariant(): void {
    let variant = new DishVariantModel();

    variant.variantId = Guid.create().toString();
    variant.name = "";
    variant.price = 0;

    this.dish.variants.push(variant);
  }

  onRemoveVariant(variant: DishVariantModel): void {
    let index = this.dish.variants.findIndex(en => en.variantId === variant.variantId);
    this.dish.variants.splice(index, 1);

    if (this.dish.variants.length === 1) {
      this.price = this.dish.variants[0].price;
    }
  }

  onSubmit(data) {
    this.blockUI.start("Verarbeite Daten...");

    this.dish.name = data.name;
    this.dish.description = data.description;
    this.dish.productInfo = data.productInfo;

    if (this.dish.variants.length === 0) {
      let variant = new DishVariantModel();
      variant.variantId = Guid.create().toString();
      variant.name = data.name;
      variant.price = this.price;
      this.dish.variants.push(variant);
    } else if (this.dish.variants.length === 1) {
      this.dish.variants[0].name = data.name;
      this.dish.variants[0].price = this.price;
    }

    console.log("Dish: ", this.dish);

    let subscription = this.restaurantAdminService.addOrChangeDishOfRestaurantAsync(this.restaurantId, this.dishCategoryId, this.dish)
      .subscribe((newDishId) => {
        subscription.unsubscribe();
        this.blockUI.stop();

        this.dish.id = newDishId;

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
