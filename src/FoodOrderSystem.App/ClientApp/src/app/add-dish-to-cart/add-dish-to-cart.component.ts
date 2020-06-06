import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {DishModel} from '../dish-category/dish.model';
import {OrderService} from '../order/order.service';
import {DishVariantModel} from '../dish-category/dish-variant.model';

@Component({
  selector: 'app-add-dish-to-cart',
  templateUrl: './add-dish-to-cart.component.html',
  styleUrls: ['./add-dish-to-cart.component.css', '../../assets/css/frontend.min.css']
})
export class AddDishToCartComponent implements OnInit {
  @Input() public dish: DishModel;

  selectedVariant: DishVariantModel;
  count: number;

  constructor(
    public activeModal: NgbActiveModal,
    private orderService: OrderService
  ) {
    this.count = 1;
  }

  ngOnInit() {
    this.selectedVariant = this.dish.variants[0];
  }

  getVariantPrice(variant: DishVariantModel): string {
    return '€' + variant.price.toLocaleString('de', { minimumFractionDigits: 2 });
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
    this.orderService.addDishVariantToCart(this.dish, this.selectedVariant, this.count);
    this.activeModal.close();
  }

  onCancel(): void {
    this.activeModal.close();
  }
}
