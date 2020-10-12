import {Component, OnDestroy, OnInit} from '@angular/core';
import {OrderService} from '../order/order.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {Subject, Subscription} from 'rxjs';
import {debounceTime, distinctUntilChanged, take} from 'rxjs/operators';
import {OrderType} from '../cart/cart.model';
import {CuisineModel} from '../cuisine/cuisine.model';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {OpeningHourFilterComponent} from '../opening-hour-filter/opening-hour-filter.component';

@Component({
  selector: 'app-order-restaurants',
  templateUrl: './order-restaurants.component.html',
  styleUrls: [
    './order-restaurants.component.css',
    '../../assets/css/frontend_v2.min.css'
  ]
})
export class OrderRestaurantsComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  cuisines: CuisineModel[];

  selectedOpeningHourFilter: Date;
  selectedCuisineFilter: string;

  restaurants: RestaurantModel[];
  restaurantCount: number;
  pageOfRestaurants: RestaurantModel[];

  orderType: string;

  private searchPhrase: string;
  private searchPhraseUpdated: Subject<string> = new Subject<string>();

  private updateSearchSubscription: Subscription;

  constructor(
    private orderService: OrderService,
    private modalService: NgbModal,
  ) {
    this.orderType = OrderService.translateFromOrderType(OrderType.Pickup);
    this.searchPhraseUpdated.asObservable().pipe(debounceTime(200), distinctUntilChanged())
      .subscribe((value: string) => {
        this.searchPhrase = value;
        this.updateSearch();
      });
  }

  ngOnInit() {
    this.selectedOpeningHourFilter = new Date(); // now
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

  openOpeningHourFilter(): void {
    const modalRef = this.modalService.open(OpeningHourFilterComponent, {centered: true});
    modalRef.componentInstance.value = this.selectedOpeningHourFilter;
    modalRef.result.then((value: Date) => {
      console.log('filtering by opening hours on date: ', value);
      this.selectedOpeningHourFilter = value;
      this.sortRestaurants();
    }, () => {
    });
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

  updateSearch(): void {
    if (this.updateSearchSubscription !== undefined) {
      this.updateSearchSubscription.unsubscribe();
    }

    this.blockUI.start('Suche läuft');
    this.orderService.searchForRestaurantsAsync(this.searchPhrase, OrderService.translateToOrderType(this.orderType),
      this.selectedCuisineFilter, undefined)
      .pipe(take(1))
      .subscribe((result) => {
        this.restaurants = new Array<RestaurantModel>(result.length);
        this.restaurantCount = result.length;

        for (let i = 0; i < result.length; i++) {
          this.restaurants[i] = new RestaurantModel(result[i]);
        }

        this.sortRestaurants();

        this.blockUI.stop();
      }, () => {
          this.blockUI.stop();
      });
  }

  getRestaurantSubText(restaurant: RestaurantModel): string {
    if (restaurant === undefined || restaurant.cuisines === undefined || restaurant.cuisines.length === 0) {
      return '';
    }
    return restaurant.cuisines.map(en => en.name).join(', ');
  }

  isRestaurantOpen(restaurant: RestaurantModel): boolean {
    const date = this.selectedOpeningHourFilter ?? new Date();
    return restaurant.isOpen(date);
  }

  sortRestaurants(): void {
    if (!this.restaurants) {
      return;
    }

    const date = this.selectedOpeningHourFilter ?? new Date();

    this.restaurants.sort((a, b) => {
      const isAOpen = a.isOpen(date);
      const isBOpen = b.isOpen(date);

      if (isAOpen && !isBOpen) {
        return -1;
      } else if (!isAOpen && isBOpen) {
        return 1;
      } else {
        if (a.name < b.name) {
          return -1;
        } else if (a.name > b.name) {
          return +1;
        } else {
          return 0;
        }
      }
    });
  }
}
