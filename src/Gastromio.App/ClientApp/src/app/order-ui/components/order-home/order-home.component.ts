import {Component, OnDestroy, OnInit} from '@angular/core';
import {Router} from '@angular/router';

import {map} from 'rxjs/operators';
import {Observable} from 'rxjs';

import {RestaurantModel} from '../../../shared/models/restaurant.model';

import {OrderFacade} from "../../../order/order.facade";

@Component({
  selector: 'app-order-home',
  templateUrl: './order-home.component.html',
  styleUrls: [
    './order-home.component.css',
    '../../../../assets/css/frontend_v3.min.css'
  ]
})
export class OrderHomeComponent implements OnInit, OnDestroy {

  restaurants$: Observable<RestaurantModel[]>;

  constructor(
    private orderFacade: OrderFacade,
    public router: Router
  ) {
  }

  ngOnInit() {
    this.orderFacade.resetFilters();

    this.restaurants$ = this.orderFacade.getRestaurants$()
      .pipe(
        map(restaurants => {
          if (this.orderFacade.getSelectedSearchPhrase().length === 0 || restaurants === undefined) {
            return new Array<RestaurantModel>();
          }

          const count = Math.min(6, restaurants.length);
          const result = new Array<RestaurantModel>(count);
          for (let i = 0; i < count; i++) {
            result[i] = restaurants[i];
          }

          return result;
        })
      );
  }

  ngOnDestroy() {
  }

  onSearchType(value: string) {
    this.orderFacade.setSelectedSearchPhrase(value);
  }

  updateSearch(): void {
    this.orderService.searchForRestaurantsAsync(this.searchPhrase, undefined, '', undefined)
      .subscribe((result) => {
        let count = Math.min(result.length, 6);

        this.restaurants = new Array<RestaurantModel>(count);

        for (let i = 0; i < count; i++) {
          this.restaurants[i] = new RestaurantModel(result[i]);
        }

  onShowAll(): void {
    this.orderFacade.resetFilters();
    this.router.navigate(['/restaurants']);
  }

  onRestaurantSelected(restaurant: RestaurantModel, orderType: string): void {
    this.router.navigate(['/restaurants', restaurant.alias], { queryParams: { orderType: orderType } });
  }

}
