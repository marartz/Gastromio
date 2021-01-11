import {Component, OnDestroy, OnInit} from '@angular/core';
import {Router} from '@angular/router';

import {debounceTime, distinctUntilChanged} from 'rxjs/operators';
import {Subject} from 'rxjs';

import {RestaurantModel} from '../../../shared/models/restaurant.model';

import {OrderService} from '../../../order/services/order.service';

@Component({
  selector: 'app-order-home',
  templateUrl: './order-home.component.html',
  styleUrls: [
    './order-home.component.css',
    '../../../../assets/css/frontend_v3.min.css'
  ]
})
export class OrderHomeComponent implements OnInit, OnDestroy {

  private searchPhrase: string;
  private searchPhraseUpdated: Subject<string> = new Subject<string>();

  restaurants: RestaurantModel[];

  constructor(
    private orderService: OrderService,
    public router: Router
  ) {
    this.searchPhraseUpdated.asObservable()
      .pipe(
        debounceTime(200),
        distinctUntilChanged()
      )
      .subscribe((value: string) => {
        if (value.length >= 3) {
          this.searchPhrase = value;
          this.updateSearch();
        } else {
          this.restaurants = new Array<RestaurantModel>();
        }
      });
  }

  ngOnInit() {
  }

  ngOnDestroy() {
  }

  onSearchType(value: string) {
    this.searchPhraseUpdated.next(value);
  }

  updateSearch(): void {
    this.orderService.searchForRestaurantsAsync(this.searchPhrase, undefined, '', undefined)
      .subscribe((result) => {
        let count = Math.min(result.length, 6);

        this.restaurants = new Array<RestaurantModel>(count);

        for (let i = 0; i < count; i++) {
          this.restaurants[i] = new RestaurantModel(result[i]);
        }

        this.sortRestaurants();
      }, () => {
      });
  }

  onRestaurantSelected(restaurant: RestaurantModel, orderType: string): void {
    this.router.navigate(['/restaurants', restaurant.alias], { queryParams: { orderType: orderType } });
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
