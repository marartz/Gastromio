import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {DishModel} from '../dish-category/dish.model';
import {OrderService} from '../order/order.service';
import {DishVariantModel} from '../dish-category/dish-variant.model';

@Component({
  selector: 'app-add-dish-to-cart',
  templateUrl: './add-dish-to-cart.component.html',
  styleUrls: ['./add-dish-to-cart.component.css']
})
export class AddDishToCartComponent implements OnInit {
  @Input() public dish: DishModel;

  selectedVariant: DishVariantModel;

  constructor(
    public activeModal: NgbActiveModal,
    private orderService: OrderService
  ) {
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

  onSubmit(): void {
    this.orderService.addDishVariantToCart(this.dish, this.selectedVariant);
    this.activeModal.close();
  }
}
