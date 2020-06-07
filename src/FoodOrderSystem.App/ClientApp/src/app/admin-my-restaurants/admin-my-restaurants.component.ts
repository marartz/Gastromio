import { Component, OnInit, OnDestroy } from '@angular/core';

import { RestaurantModel } from '../restaurant/restaurant.model';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';

import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-my-restaurants',
  templateUrl: './admin-my-restaurants.component.html',
  styleUrls: ['./admin-my-restaurants.component.css', '../../assets/css/frontend.min.css']
})
export class AdminMyRestaurantsComponent implements OnInit, OnDestroy {
  restaurants: RestaurantModel[];

  constructor(
    private restaurantAdminService: RestaurantRestAdminService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    this.updateSearch();
  }

  ngOnDestroy() {
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['']);
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
