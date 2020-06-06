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

  constructor(
    public activeModal: NgbActiveModal,
    private orderService: OrderService
  ) {
  }

  ngOnInit() {
  }

  getVariantPrice(variant: DishVariantModel): string {
    return '€' + variant.price.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  getVariantText(variant: DishVariantModel): string {
    return variant.name + ' ' + this.getVariantPrice(variant);
  }

  incCount(): void {
    this.orderService.incrementDishVariantCount(this.orderedDish.itemId);
  }

  decCount(): void {
    if (this.orderedDish.count <= 1) {
      return;
    }
    this.orderService.decrementDishVariantCount(this.orderedDish.itemId);
  }

  onClose(): void {
    this.activeModal.close();
  }

  onRemove(): void {
    this.orderService.removeDishVariantFromCart(this.orderedDish.itemId);
    this.activeModal.close();
  }
}
