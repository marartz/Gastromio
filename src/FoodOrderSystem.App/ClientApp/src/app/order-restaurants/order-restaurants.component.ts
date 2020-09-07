import {Component, OnDestroy, OnInit} from '@angular/core';
import {OrderService} from '../order/order.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {BehaviorSubject, Subject, Subscription} from 'rxjs';
import {debounceTime, distinctUntilChanged, take} from 'rxjs/operators';
import {OrderType} from '../cart/cart.model';
import {CuisineModel} from '../cuisine/cuisine.model';

@Component({
  selector: 'app-order-restaurants',
  templateUrl: './order-restaurants.component.html',
  styleUrls: ['./order-restaurants.component.css', '../../assets/css/frontend_v2.min.css']
})
export class OrderRestaurantsComponent implements OnInit, OnDestroy {
  cuisines: CuisineModel[];

  openingHourFilter$ = new BehaviorSubject<string>(undefined);
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

    this.openingHourFilter$.next(new Date().toISOString());
    this.openingHourFilter$
      .pipe(
        debounceTime(200)
      )
      .subscribe(() => {
        console.log('updateSearch');
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

  hasBanner(
    restaurant: RestaurantModel
  ): boolean {
    return restaurant.imageTypes.some(en => en === 'banner');
  }

  getBannerStyle(
    restaurant: RestaurantModel
  ): string {
    if (!restaurant) {
      return undefined;
    }
    return '/api/v1/restaurants/' + restaurant.id + '/images/banner';
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

  onOpeningHourFilterChanged(openingHourFilter: string): void {
    this.openingHourFilter$.next(openingHourFilter);
  }

  updateSearch(): void {
    if (this.updateSearchSubscription !== undefined) {
      this.updateSearchSubscription.unsubscribe();
    }

    this.orderService.searchForRestaurantsAsync(this.searchPhrase, OrderService.translateToOrderType(this.orderType),
      this.selectedCuisineFilter, undefined)
      .pipe(take(1))
      .subscribe((result) => {

        this.restaurants = new Array<RestaurantModel>(result.length);
        for (let i = 0; i < result.length; i++) {
          this.restaurants[i] = new RestaurantModel(result[i]);
        }
      }, () => {
      });
  }

  getRestaurantSubText(restaurant: RestaurantModel): string {
    if (restaurant === undefined || restaurant.cuisines === undefined || restaurant.cuisines.length === 0) {
      return '';
    }
    return restaurant.cuisines.map(en => en.name).join(', ');
  }

  isRestaurantOpen(restaurant: RestaurantModel): boolean {
    if (!restaurant.openingHours) {
      return false;
    }

    try {
      const date = new Date(this.openingHourFilter$.getValue());

      let dayOfWeek = (date.getDay() - 1) % 7; // DayOfWeek starts with Sunday
      if (dayOfWeek < 0) {
        dayOfWeek += 7;
      }
      let time = date.getHours() * 60 + date.getMinutes();
      if (date.getHours() < 4) {
        dayOfWeek = (dayOfWeek - 1) % 7;
        if (dayOfWeek < 0) {
          dayOfWeek += 7;
        }
        time += 24 * 60;
      }

      let isOpen: boolean;
      isOpen = restaurant.openingHours.some(en => en.dayOfWeek === dayOfWeek && en.start <= time && time <= en.end);

      return isOpen;
    } catch (e) {
      console.error(e);
      return false;
    }
  }
}