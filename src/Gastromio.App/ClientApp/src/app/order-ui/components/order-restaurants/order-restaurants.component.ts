import {Component, OnDestroy, OnInit} from '@angular/core';

import {Observable, Subject, Subscription} from 'rxjs';
import {debounceTime, distinctUntilChanged, map, take} from 'rxjs/operators';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {RestaurantModel} from '../../../shared/models/restaurant.model';
import {CuisineModel} from '../../../shared/models/cuisine.model';

import {OrderFacade} from "../../../order/order.facade";
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

  selectedOrderType$: Observable<string>;
  selectedOpeningHour$: Observable<Date>;
  selectedOpeningHourText$: Observable<string>;

  showMobileFilterDetails: boolean;

  totalRestaurantCount: number;

  openedRestaurants: RestaurantModel[];
  openedRestaurantCount: number;
  closedRestaurants: RestaurantModel[];
  closedRestaurantCount: number;

  private searchPhrase: string;
  private searchPhraseUpdated: Subject<string> = new Subject<string>();

  private updateSearchSubscription: Subscription;

  constructor(
    private orderFacade: OrderFacade,
    private orderService: OrderService,
    private modalService: NgbModal,
  ) {
    this.searchPhraseUpdated.asObservable().pipe(debounceTime(200), distinctUntilChanged())
      .subscribe((value: string) => {
        this.searchPhrase = value;
        this.updateSearch();
      });
  }

  ngOnInit() {
    this.selectedOrderType$ = this.orderFacade.getSelectedOrderType$()
      .pipe(
        map(selectedOrderType => OrderService.translateFromOrderType(selectedOrderType))
      );

    this.selectedOpeningHour$ = this.orderFacade.getSelectedOpeningHour$();

    this.selectedOpeningHourText$ = this.selectedOpeningHour$
      .pipe(
        map(selectedOpeningHourFilter => {
          if (!selectedOpeningHourFilter)
            return undefined;

          let hoursText = selectedOpeningHourFilter.getHours().toString();
          hoursText = ('0' + hoursText).slice(-2);

          let minutesText = selectedOpeningHourFilter.getMinutes().toString();
          minutesText = ('0' + minutesText).slice(-2);

          return selectedOpeningHourFilter.toLocaleDateString() + ', ' + hoursText + ':' + minutesText;
        })
      );

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
    modalRef.componentInstance.value = this.orderFacade.getSelectedOpeningHour() ?? new Date();
    modalRef.result.then((value: Date) => {
      this.orderFacade.setSelectedOpeningHour(value);
      this.updateSearch();
    }, () => {
    });
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
    this.orderFacade.setSelectedOrderType(OrderType.Delivery);
    this.updateSearch();
  }

  onPickupSelected(): void {
    this.orderFacade.setSelectedOrderType(OrderType.Pickup);
    this.updateSearch();
  }

  onSearchType(value: string) {
    this.searchPhraseUpdated.next(value);
  }

  isCuisineSelected(cuisine: CuisineModel): boolean {
    const selectedCuisineFilter = this.orderFacade.getSelectedCuisine();
    if (cuisine) {
      return selectedCuisineFilter === cuisine.id;
    } else {
      return selectedCuisineFilter === '';
    }
  }

  selectCuisine(cuisine: CuisineModel): void {
    if (!cuisine) {
      this.orderFacade.setSelectedCuisine('');
    }
    else
    {
      this.orderFacade.setSelectedCuisine(cuisine.id);
    }
    this.updateSearch();
  }

  updateSearch(): void {
    if (this.updateSearchSubscription !== undefined) {
      this.updateSearchSubscription.unsubscribe();
    }

    this.blockUI.start('Suche läuft');

    this.orderService.searchForRestaurantsAsync(this.searchPhrase, this.orderFacade.getSelectedOrderType(), this.orderFacade.getSelectedCuisine(), null)
      .pipe(take(1))
      .subscribe((result) => {
        let restaurants = new Array<RestaurantModel>(result.length);
        this.totalRestaurantCount = result.length;

        for (let i = 0; i < result.length; i++) {
          restaurants[i] = new RestaurantModel(result[i]);
        }

        const selectedOpeningHourFilter = this.orderFacade.getSelectedOpeningHour();

        let openedRestaurants = new Array<RestaurantModel>();
        let closedRestaurants = new Array<RestaurantModel>();
        for (let restaurant of restaurants) {
          if (restaurant.isOpen(selectedOpeningHourFilter)) {
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
