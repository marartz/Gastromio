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
    '../../assets/css/frontend_v3.min.css'
  ]
})
export class OrderRestaurantsComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  cuisines: CuisineModel[];

  showMobileFilterDetails: boolean;

  selectedOpeningHourFilter: Date;
  selectedCuisineFilter: string;
  showClosedRestaurants: boolean;

  restaurants: RestaurantModel[];
  restaurantCount: number;

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
    this.showClosedRestaurants = false;

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
    modalRef.componentInstance.value = this.selectedOpeningHourFilter;
    modalRef.result.then((value: Date) => {
      console.log('filtering by opening hours on date: ', value);
      this.selectedOpeningHourFilter = value;
      this.updateSearch();
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
      this.selectedCuisineFilter, this.showClosedRestaurants ? null : date.toISOString())
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

  onJustShowOpenRestaurants(): void {
    this.showClosedRestaurants = false;
    this.updateSearch();
  }

  onShowAllRestaurants(): void {
    this.showClosedRestaurants = true;
    this.updateSearch();
  }

  sortRestaurants(): void {
    if (!this.restaurants) {
      return;
    }

    this.restaurants.sort((a, b) => {
      if (a.name < b.name) {
        return -1;
      } else if (a.name > b.name) {
        return +1;
      } else {
        return 0;
      }
    });
  }
}
