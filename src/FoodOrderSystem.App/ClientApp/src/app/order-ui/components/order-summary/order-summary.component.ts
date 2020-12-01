import {Component, OnInit} from '@angular/core';

import {RestaurantModel} from '../../../shared/models/restaurant.model';

import {OrderModel} from '../../../order/models/order.model';

import {OrderService} from '../../../order/services/order.service';

@Component({
  selector: 'app-order-summary',
  templateUrl: './order-summary.component.html',
  styleUrls: ['./order-summary.component.css', '../../../../assets/css/frontend_v3.min.css']
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

  hasLogo(): boolean {
    return this.restaurant?.imageTypes.some(en => en === 'logo');
  }

  getLogoUrl(): string {
    if (!this.restaurant) {
      return undefined;
    }
    return '/api/v1/restaurants/' + this.restaurant.id + '/images/logo';
  }
}
