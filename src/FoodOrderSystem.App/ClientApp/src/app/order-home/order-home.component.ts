import {Component, OnDestroy, OnInit} from '@angular/core';
import {OrderService} from '../order/order.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {Observable, of} from 'rxjs';
import {debounceTime, distinctUntilChanged, switchMap, take} from 'rxjs/operators';
import {Router} from '@angular/router';
import {OrderType} from '../cart/cart.model';

@Component({
  selector: 'app-order-home',
  templateUrl: './order-home.component.html',
  styleUrls: ['./order-home.component.css', '../../assets/css/frontend.min.css']
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

  onDeliverySelected(): void {
    this.orderType = 'delivery';
  }

  onPickupSelected(): void {
    this.orderType = 'pickup';
  }

  onReservationSelected(): void {
    this.orderType = 'reservation';
  }

  searchForRestaurant = (text: Observable<string>) =>
    text.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2
        ? of([])
        : this.orderService.searchForRestaurantsAsync(term, OrderService.translateToOrderType(this.orderType))),
      take(10)
    )

  formatRestaurant(restaurant: RestaurantModel): string {
    return restaurant.name;
  }

  onRestaurantSelected(restaurant: RestaurantModel): void {
    this.router.navigate(['/restaurants', restaurant.id], { queryParams: { orderType: this.orderType } });
  }
}
