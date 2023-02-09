import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { DishModel } from '../../../shared/models/dish.model';
import { DishVariantModel } from '../../../shared/models/dish-variant.model';

import { OrderFacade } from '../../../order/order.facade';

@Component({
  selector: 'app-add-dish-to-cart',
  templateUrl: './add-dish-to-cart.component.html',
  styleUrls: ['./add-dish-to-cart.component.css', '../../../../assets/css/frontend_v3.min.css', '../../../../assets/css/modals.component.min.css'],
})
export class AddDishToCartComponent implements OnInit {
  @Input() public dish: DishModel;

  selectedVariant: DishVariantModel;
  count: number;
  remarks: string;

  constructor(public activeModal: NgbActiveModal, private orderFacade: OrderFacade) {
    this.count = 1;
  }

  ngOnInit() {
    this.selectedVariant = this.dish.variants[0];
  }

  getVariantPrice(variant: DishVariantModel): string {
    return '€' + variant.price.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  getTotalPrice(): string {
    const totalPrice = this.selectedVariant.price * this.count;
    return totalPrice.toLocaleString('de', { minimumFractionDigits: 2 }) + ' €';
  }

  getVariantText(variant: DishVariantModel): string {
    return variant.name + ' ' + this.getVariantPrice(variant);
  }

  incCount(): void {
    this.count++;
  }

  decCount(): void {
    if (this.count <= 1) {
      return;
    }
    this.count--;
  }

  onSubmit(): void {
    this.orderFacade.addDishToCart(this.dish, this.selectedVariant, this.count, this.remarks);
    this.activeModal.close();
  }

  onCancel(): void {
    this.activeModal.close();
  }
}
