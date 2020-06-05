import { Component, OnInit, OnDestroy } from '@angular/core';
import { OrderService } from '../order/order.service';
import { RestaurantModel } from '../restaurant/restaurant.model';
import { Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, take } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-order-home',
  templateUrl: './order-home.component.html',
  styleUrls: ['./order-home.component.css', '../../assets/css/frontend.min.css'] 
})
export class OrderHomeComponent implements OnInit, OnDestroy {
  selectedRestaurant: RestaurantModel;

  constructor(
    private orderService: OrderService,
    public router: Router
  ) {
  }

  ngOnInit() {
  }

  ngOnDestroy() {
  }

  searchForRestaurant = (text: Observable<string>) =>
    text.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? of([]) : this.orderService.searchForRestaurantsAsync(term)),
      take(10)
    )

  formatRestaurant(restaurant: RestaurantModel): string {
    return restaurant.name;
  }

  onRestaurantSelected(restaurant: RestaurantModel): void {
    console.log('restaurant selected:', restaurant);
    this.router.navigate(['/restaurants', restaurant.id]);
  }
}
