import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';

import {RestaurantAdminAuthGuardService} from '../auth/services/restaurant-admin-auth-guard.service';

import {AdminMyRestaurantsComponent} from './components/admin-my-restaurants/admin-my-restaurants.component';
import {AdminRestaurantComponent} from './components/admin-restaurant/admin-restaurant.component';

const routes: Routes = [
  {path: 'admin/myrestaurants', component: AdminMyRestaurantsComponent, canActivate: [RestaurantAdminAuthGuardService]},
  {path: 'admin/restaurants/:restaurantId', component: AdminRestaurantComponent, canActivate: [RestaurantAdminAuthGuardService]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RestaurantAdminRoutingModule {
}
