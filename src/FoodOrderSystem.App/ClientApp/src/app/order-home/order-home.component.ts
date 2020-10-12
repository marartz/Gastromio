import {Component, OnDestroy, OnInit} from '@angular/core';
import {OrderService} from '../order/order.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {Router} from '@angular/router';
import {OrderType} from '../cart/cart.model';

@Component({
  selector: 'app-order-home',
  templateUrl: './order-home.component.html',
  styleUrls: [
    './order-home.component.css',
    '../../assets/css/frontend_v3.min.css'
  ]
})
export class OrderHomeComponent implements OnInit, OnDestroy {
  selectedRestaurant: RestaurantModel;

  orderType: string;

  constructor(
    private orderService: OrderService,
    public router: Router
  ) {
    this.orderType = OrderService.translateFromOrderType(OrderType.Pickup);
  }

  ngOnInit() {
  }

  ngOnDestroy() {
  }

  onRestaurantSelected(restaurant: RestaurantModel): void {
    this.router.navigate(['/restaurants', restaurant.id], { queryParams: { orderType: this.orderType } });
  }
}