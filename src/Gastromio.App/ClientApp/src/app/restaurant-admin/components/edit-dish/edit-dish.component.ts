import {Component, OnInit, Input} from '@angular/core';
import {FormGroup, FormBuilder, Validators} from '@angular/forms';

import {Observable} from "rxjs";

import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {Guid} from 'guid-typescript';

import {DishModel} from '../../../shared/models/dish.model';
import {DishVariantModel} from '../../../shared/models/dish-variant.model';

import {RestaurantAdminFacade} from "../../restaurant-admin.facade";

@Component({
  selector: 'app-edit-dish',
  templateUrl: './edit-dish.component.html',
  styleUrls: [
    './edit-dish.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class EditDishComponent implements OnInit {
  @Input() public dishCategoryId: string;
  @Input() public dish: DishModel;
  @BlockUI() blockUI: NgBlockUI;

  isNew: boolean;
  editDishForm: FormGroup;
  message$: Observable<string>;
  submitted = false;

  price: number;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private facade: RestaurantAdminFacade
  ) {
  }

  ngOnInit() {
    this.message$ = this.facade.getUpdateError$();

    if (this.dish === undefined) {
      this.dish = new DishModel();
      this.dish.variants = new Array<DishVariantModel>();
    }

    this.isNew = this.dish.id === undefined;

    this.editDishForm = this.formBuilder.group({
      name: [this.dish.name, Validators.required],
      description: [this.dish.description],
      productInfo: [this.dish.productInfo],
    });

    if (this.dish.variants.length === 0) {
      this.price = 0;
    } else if (this.dish.variants.length === 1) {
      this.price = this.dish.variants[0].price;
    }
  }

  onAddVariant(): void {
    const variant = new DishVariantModel();

    variant.variantId = Guid.create().toString();
    variant.name = '';
    variant.price = 0;

    this.dish.variants.push(variant);
  }

  onRemoveVariant(variant: DishVariantModel): void {
    const index = this.dish.variants.findIndex(en => en.variantId === variant.variantId);
    this.dish.variants.splice(index, 1);

    if (this.dish.variants.length === 1) {
      this.price = this.dish.variants[0].price;
    }
  }

  get f() {
    return this.editDishForm.controls;
  }

  onSubmit(data) {
    this.submitted = true;
    if (this.editDishForm.invalid) {
      return;
    }

    this.dish.name = data.name;
    this.dish.description = data.description;
    this.dish.productInfo = data.productInfo;

    if (this.dish.variants.length === 0) {
      const variant = new DishVariantModel();
      variant.variantId = Guid.create().toString();
      variant.name = data.name;
      variant.price = this.price;
      this.dish.variants.push(variant);
    } else if (this.dish.variants.length === 1) {
      this.dish.variants[0].name = data.name;
      this.dish.variants[0].price = this.price;
    }

    this.facade.addOrChangedDish(this.dishCategoryId, this.dish)
      .subscribe(() => {
        this.activeModal.close();
      })
  }

}
