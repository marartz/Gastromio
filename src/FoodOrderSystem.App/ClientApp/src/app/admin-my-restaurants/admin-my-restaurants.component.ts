import {Component, OnInit, OnDestroy} from '@angular/core';

import {RestaurantModel} from '../restaurant/restaurant.model';
import {RestaurantRestAdminService} from '../restaurant-rest-admin/restaurant-rest-admin.service';

import {AuthService} from '../auth/auth.service';
import {Router} from '@angular/router';
import {take} from 'rxjs/operators';

@Component({
  selector: 'app-admin-my-restaurants',
  templateUrl: './admin-my-restaurants.component.html',
  styleUrls: [
    './admin-my-restaurants.component.css',
    '../../assets/css/frontend_v2.min.css',
    '../../assets/css/backend_v2.min.css',
    '../../assets/css/animations_v2.min.css'
  ]
})
export class AdminMyRestaurantsComponent implements OnInit, OnDestroy {
  restaurants: RestaurantModel[];

  constructor(
    private restaurantAdminService: RestaurantRestAdminService,
    private authService: AuthService,
    private router: Router
  ) {
  }

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
    this.restaurantAdminService.getMyRestaurantsAsync()
      .pipe(take(1))
      .subscribe((result) => {
        this.restaurants = result;
      }, (error) => {
        // TODO
      });
  }
}