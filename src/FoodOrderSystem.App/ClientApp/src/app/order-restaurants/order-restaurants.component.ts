import {Component, OnDestroy, OnInit} from '@angular/core';
import {OrderService} from '../order/order.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {Subject, Subscription} from 'rxjs';
import {debounceTime, distinctUntilChanged, take} from 'rxjs/operators';
import {OrderType} from '../cart/cart.model';
import {CuisineModel} from '../cuisine/cuisine.model';

@Component({
  selector: 'app-order-restaurants',
  templateUrl: './order-restaurants.component.html',
  styleUrls: ['./order-restaurants.component.css', '../../assets/css/frontend.min.css']
})
export class OrderRestaurantsComponent implements OnInit, OnDestroy {
  cuisines: CuisineModel[];
  selectedCuisineFilter: string;

  restaurants: RestaurantModel[];
  pageOfRestaurants: RestaurantModel[];

  orderType: string;

  private searchPhrase: string;
  private searchPhraseUpdated: Subject<string> = new Subject<string>();

  private updateSearchSubscription: Subscription;

  constructor(
    private orderService: OrderService
  ) {
    this.orderType = OrderService.translateFromOrderType(OrderType.Pickup);
    this.searchPhraseUpdated.asObservable().pipe(debounceTime(200), distinctUntilChanged())
      .subscribe((value: string) => {
        this.searchPhrase = value;
        this.updateSearch();
      });
  }

  ngOnInit() {
    this.selectedCuisineFilter = '';
    this.orderService.getAllCuisinesAsync()
      .pipe(take(1))
      .subscribe((cuisines) => {
        this.cuisines = cuisines;
      });

    this.searchPhrase = '';
    this.updateSearch();
  }

  ngOnDestroy() {
    if (this.updateSearchSubscription !== undefined) {
      this.updateSearchSubscription.unsubscribe();
    }
  }

  hasLogo(
    restaurant: RestaurantModel
  ): boolean {
    return restaurant.imageTypes.some(en => en === 'logo');
  }

  getLogoUrl(
    restaurant: RestaurantModel
  ): string {
    if (!restaurant) {
      return undefined;
    }
    return '/api/v1/restaurants/' + restaurant.id + '/images/logo';
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

  onSelectedCuisineFilterChanged(): void {
    this.updateSearch();
  }

  updateSearch(): void {
    if (this.updateSearchSubscription !== undefined) {
      this.updateSearchSubscription.unsubscribe();
    }

    this.orderService.searchForRestaurantsAsync(this.searchPhrase, OrderService.translateToOrderType(this.orderType),
      this.selectedCuisineFilter)
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
