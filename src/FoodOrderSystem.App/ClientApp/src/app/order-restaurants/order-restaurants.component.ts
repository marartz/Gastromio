import {Component, OnDestroy, OnInit} from '@angular/core';
import {OrderService} from '../order/order.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {Subject, Subscription} from 'rxjs';
import {debounceTime, distinctUntilChanged, take} from 'rxjs/operators';
import {OrderType} from '../cart/cart.model';

@Component({
  selector: 'app-order-restaurants',
  templateUrl: './order-restaurants.component.html',
  styleUrls: ['./order-restaurants.component.css', '../../assets/css/frontend.min.css']
})
export class OrderRestaurantsComponent implements OnInit, OnDestroy {
  restaurants: RestaurantModel[];
  pageOfRestaurants: RestaurantModel[];

  orderType: string;

  private searchPhrase: string;
  private searchPhraseUpdated: Subject<string> = new Subject<string>();

  private updateSearchSubscription: Subscription;

  constructor(
    private orderService: OrderService
  ) {
    this.orderType = OrderService.translateFromOrderType(OrderType.Delivery);
    this.searchPhraseUpdated.asObservable().pipe(debounceTime(200), distinctUntilChanged())
      .subscribe((value: string) => {
        this.searchPhrase = value;
        this.updateSearch();
      });
  }

  ngOnInit() {
    this.searchPhrase = '';
    this.updateSearch();
  }

  ngOnDestroy() {
    if (this.updateSearchSubscription !== undefined) {
      this.updateSearchSubscription.unsubscribe();
    }
  }

  onDeliverySelected(): void {
    this.orderType = 'delivery';
    this.updateSearch();
  }

  onPickupSelected(): void {
    this.orderType = 'pickup';
    this.updateSearch();
  }

  onReservationSelected(): void {
    this.orderType = 'reservation';
    this.updateSearch();
  }

  onSearchType(value: string) {
    this.searchPhraseUpdated.next(value);
  }

  onChangePage(pageOfRestaurants: RestaurantModel[]) {
    this.pageOfRestaurants = pageOfRestaurants;
  }

  updateSearch(): void {
    if (this.updateSearchSubscription !== undefined) {
      this.updateSearchSubscription.unsubscribe();
    }

    this.orderService.searchForRestaurantsAsync(this.searchPhrase, OrderService.translateToOrderType(this.orderType))
      .pipe(take(1))
      .subscribe((result) => {
        this.restaurants = result;
      }, () => {
      });
  }

  getRestaurantSubText(restaurant: RestaurantModel): string {
    if (restaurant === undefined || restaurant.cuisines === undefined || restaurant.cuisines.length === 0) {
      return '';
    }
    return restaurant.cuisines.map(en => en.name).join(', ');
  }
}
