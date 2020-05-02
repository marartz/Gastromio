import { Component, OnInit, OnDestroy } from '@angular/core';
import { OrderService } from '../order/order.service';
import { RestaurantModel } from '../restaurant/restaurant.model';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-order-home',
  templateUrl: './order-home.component.html',
  styleUrls: ['./order-home.component.css']
})
export class OrderHomeComponent implements OnInit, OnDestroy {
  restaurants: RestaurantModel[];
  pageOfRestaurants: RestaurantModel[];

  private searchPhrase: string;
  private searchPhraseUpdated: Subject<string> = new Subject<string>();

  private updateSearchSubscription: Subscription;

  constructor(
    private orderService: OrderService
  ) {
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

  onSearchType(value: string) {
    this.searchPhraseUpdated.next(value);
  }

  onChangePage(pageOfRestaurants: RestaurantModel[]) {
    this.pageOfRestaurants = pageOfRestaurants;
  }

  updateSearch(): void {
    if (this.updateSearchSubscription !== undefined)
      this.updateSearchSubscription.unsubscribe();

    let observable = this.orderService.searchForRestaurantsAsync(this.searchPhrase);

    this.updateSearchSubscription = observable.subscribe((result) => {
      this.restaurants = result;
    }, (error) => {
    });
  }
}
