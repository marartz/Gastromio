import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {DishVariantModel} from '../../../shared/models/dish-variant.model';

import {OrderService} from '../../../order/services/order.service';
import {CartDishModel} from '../../../order/models/cart-dish.model';

@Component({
  selector: 'app-edit-cart-dish',
  templateUrl: './edit-cart-dish.component.html',
  styleUrls: [
    './edit-cart-dish.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class EditCartDishComponent implements OnInit {
  @Input() public cartDish: CartDishModel;

  count: number;
  remarks: string;

  constructor(
    public activeModal: NgbActiveModal,
    private orderService: OrderService
  ) {
  }

  ngOnInit() {
    this.count = this.cartDish.getCount();
    this.remarks = this.cartDish.getRemarks();
  }

  getVariantPrice(variant: DishVariantModel): string {
    return '€' + variant.price.toLocaleString('de', {minimumFractionDigits: 2});
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
    this.orderService.setCountOfCartDish(this.cartDish.getItemId(), this.count);
    this.orderService.changeRemarksOfCartDish(this.cartDish.getItemId(), this.remarks);
    this.activeModal.close();
  }

  onRemove(): void {
    this.orderService.removeCartDishFromCart(this.cartDish.getItemId());
    this.activeModal.close();
  }

  onCancel(): void {
    this.activeModal.close();
  }
}
