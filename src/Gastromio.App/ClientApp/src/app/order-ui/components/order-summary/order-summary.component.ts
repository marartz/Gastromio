import {Component, OnInit} from '@angular/core';

import {RestaurantModel} from '../../../shared/models/restaurant.model';

import {OrderModel} from '../../../order/models/order.model';

import {OrderFacade} from "../../../order/order.facade";

@Component({
  selector: 'app-order-summary',
  templateUrl: './order-summary.component.html',
  styleUrls: ['./order-summary.component.css', '../../../../assets/css/frontend_v3.min.css']
})
export class OrderSummaryComponent implements OnInit {
  restaurant: RestaurantModel;
  order: OrderModel;

  constructor(
    private orderFacade: OrderFacade
  ) {
  }

  ngOnInit() {
    this.restaurant = this.orderFacade.getSelectedRestaurant();
    this.order = this.orderFacade.getOrder();
  }
}
