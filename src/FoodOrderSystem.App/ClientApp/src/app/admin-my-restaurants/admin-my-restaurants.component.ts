import { Component, OnInit, OnDestroy } from '@angular/core';

import { RestaurantModel } from '../restaurant/restaurant.model';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';

@Component({
  selector: 'app-admin-my-restaurants',
  templateUrl: './admin-my-restaurants.component.html',
  styleUrls: ['./admin-my-restaurants.component.css']
})
export class AdminMyRestaurantsComponent implements OnInit, OnDestroy {
  restaurants: RestaurantModel[];

  constructor(
    private restaurantAdminService: RestaurantRestAdminService
  ) { }

  ngOnInit() {
    this.updateSearch();
  }

  ngOnDestroy() {
  }


  updateSearch(): void {
    const subscription = this.restaurantAdminService.getMyRestaurantsAsync().subscribe((result) => {
      subscription.unsubscribe();
      this.restaurants = result;
    }, (error) => {
        subscription.unsubscribe();
        // TODO
    });
  }
}
