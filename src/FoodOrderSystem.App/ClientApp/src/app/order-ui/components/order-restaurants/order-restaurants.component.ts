import {Component, OnDestroy, OnInit} from '@angular/core';

import {Subject, Subscription} from 'rxjs';
import {debounceTime, distinctUntilChanged, take} from 'rxjs/operators';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {RestaurantModel} from '../../../shared/models/restaurant.model';
import {CuisineModel} from '../../../shared/models/cuisine.model';

import {OrderType} from '../../../order/models/cart.model';
import {OrderService} from '../../../order/services/order.service';

import {OpeningHourFilterComponent} from '../opening-hour-filter/opening-hour-filter.component';

@Component({
  selector: 'app-order-restaurants',
  templateUrl: './order-restaurants.component.html',
  styleUrls: [
    './order-restaurants.component.css',
    '../../../../assets/css/frontend_v3.min.css'
  ]
})
export class OrderRestaurantsComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  cuisines: CuisineModel[];

  showMobileFilterDetails: boolean;

  selectedOpeningHourFilter: Date;
  selectedCuisineFilter: string;

  totalRestaurantCount: number;

  openedRestaurants: RestaurantModel[];
  openedRestaurantCount: number;
  closedRestaurants: RestaurantModel[];
  closedRestaurantCount: number;

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

  onToggleMobileFilterDetails(): void {
    this.showMobileFilterDetails = !this.showMobileFilterDetails;
  }

  onHideMobileFilterDetails(): void {
    this.showMobileFilterDetails = false;
  }

  openOpeningHourFilter(): void {
    const modalRef = this.modalService.open(OpeningHourFilterComponent, {centered: true});
    modalRef.componentInstance.value = this.selectedOpeningHourFilter ?? new Date();
    modalRef.result.then((value: Date) => {
      this.selectedOpeningHourFilter = value;
      this.updateSearch();
    }, () => {
    });
  }

  getOpeningHourFilterText(): string {
    if (!this.selectedOpeningHourFilter)
      return undefined;

    let hoursText = this.selectedOpeningHourFilter.getHours().toString();
    hoursText = ('0' + hoursText).slice(-2);

    let minutesText = this.selectedOpeningHourFilter.getMinutes().toString();
    minutesText = ('0' + minutesText).slice(-2);

    return this.selectedOpeningHourFilter.toLocaleDateString() + ', ' + hoursText + ':' + minutesText;
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

  isCuisineSelected(cuisine: CuisineModel): boolean {
    if (cuisine) {
      return this.selectedCuisineFilter === cuisine.id;
    } else {
      return this.selectedCuisineFilter === '';
    }
  }

  selectCuisine(cuisine: CuisineModel): void {
    if (!cuisine) {
      this.selectedCuisineFilter = '';
    }
    else
    {
      this.selectedCuisineFilter = cuisine.id;
    }
    this.updateSearch();
  }

  updateSearch(): void {
    if (this.updateSearchSubscription !== undefined) {
      this.updateSearchSubscription.unsubscribe();
    }

    this.blockUI.start('Suche läuft');

    const date = this.selectedOpeningHourFilter ?? new Date();
    this.orderService.searchForRestaurantsAsync(this.searchPhrase, OrderService.translateToOrderType(this.orderType),
      this.selectedCuisineFilter, null)
      .pipe(take(1))
      .subscribe((result) => {

        let restaurants = new Array<RestaurantModel>(result.length);
        this.totalRestaurantCount = result.length;

        for (let i = 0; i < result.length; i++) {
          restaurants[i] = new RestaurantModel(result[i]);
        }

        let openedRestaurants = new Array<RestaurantModel>();
        let closedRestaurants = new Array<RestaurantModel>();
        for (let restaurant of restaurants) {
          if (restaurant.isOpen(this.selectedOpeningHourFilter)) {
            openedRestaurants.push(restaurant);
          } else {
            closedRestaurants.push(restaurant);
          }
        }

        this.openedRestaurants = openedRestaurants;
        this.openedRestaurantCount = openedRestaurants.length;

        this.closedRestaurants = closedRestaurants;
        this.closedRestaurantCount = closedRestaurants.length;

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
    const compareFn = (a: RestaurantModel, b: RestaurantModel) => {
      if (a.name < b.name) {
        return -1;
      } else if (a.name > b.name) {
        return +1;
      } else {
        return 0;
      }
    };

    if (this.openedRestaurants) {
      this.openedRestaurants.sort(compareFn);
    }

    if (this.closedRestaurants) {
      this.closedRestaurants.sort(compareFn);
    }
  }
}
