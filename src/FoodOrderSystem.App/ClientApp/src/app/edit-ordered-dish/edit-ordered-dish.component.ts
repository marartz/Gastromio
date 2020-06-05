import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {OrderService} from '../order/order.service';
import {DishVariantModel} from '../dish-category/dish-variant.model';
import {OrderedDishModel} from '../cart/ordered-dish.model';

@Component({
  selector: 'app-edit-ordered-dish',
  templateUrl: './edit-ordered-dish.component.html',
  styleUrls: ['./edit-ordered-dish.component.css']
})
export class EditOrderedDishComponent implements OnInit {
  @Input() public orderedDish: OrderedDishModel;

  selectedVariant: DishVariantModel;

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
}
