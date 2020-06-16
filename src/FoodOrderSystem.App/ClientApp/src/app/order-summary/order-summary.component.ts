import {Component, OnInit} from '@angular/core';
import {OrderService} from '../order/order.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {OrderModel} from '../order/order.model';

@Component({
  selector: 'app-order-summary',
  templateUrl: './order-summary.component.html',
  styleUrls: ['./order-summary.component.css', '../../assets/css/frontend.min.css']
})
export class OrderSummaryComponent implements OnInit {
  restaurant: RestaurantModel;
  order: OrderModel;

  constructor(
    private orderService: OrderService
  ) {
  }

  ngOnInit() {
    this.restaurant = this.orderService.getRestaurant();
    this.order = this.orderService.getOrder();
  }

  formatNumber(value: number): string {
    return value.toLocaleString('de', {minimumFractionDigits: 2});
  }
}
