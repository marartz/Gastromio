import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {OrderService} from '../order/order.service';
import {DishVariantModel} from '../dish-category/dish-variant.model';
import {OrderedDishModel} from '../cart/ordered-dish.model';

@Component({
  selector: 'app-edit-ordered-dish',
  templateUrl: './edit-ordered-dish.component.html',
  styleUrls: ['./edit-ordered-dish.component.css', '../../assets/css/frontend.min.css']
})
export class EditOrderedDishComponent implements OnInit {
  @Input() public orderedDish: OrderedDishModel;

  count: number;
  remarks: string;

  constructor(
    public activeModal: NgbActiveModal,
    private orderService: OrderService
  ) {
  }

  ngOnInit() {
    this.count = this.orderedDish.count;
    this.remarks = this.orderedDish.remarks;
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
    this.orderedDish.count = this.count;
    this.orderedDish.remarks = this.remarks;
    this.activeModal.close();
  }

  onRemove(): void {
    const cart = this.orderService.getCart();
    if (!cart) {
      return;
    }
    cart.removeOrderedDish(this.orderedDish.itemId);
    this.activeModal.close();
  }

  onCancel(): void {
    this.activeModal.close();
  }
}
