import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';

import {SystemAdminAuthGuardService} from "../auth/services/system-admin-auth-guard.service";

import {AdminCuisinesComponent} from "./components/admin-cuisines/admin-cuisines.component";
import {AdminDishImportComponent} from "./components/admin-dish-import/admin-dish-import.component";
import {AdminRestaurantImportComponent} from "./components/admin-restaurant-import/admin-restaurant-import.component";
import {AdminRestaurantsComponent} from "./components/admin-restaurants/admin-restaurants.component";
import {AdminUsersComponent} from "./components/admin-users/admin-users.component";

const routes: Routes = [
  {path: 'admin/users', component: AdminUsersComponent, canActivate: [SystemAdminAuthGuardService]},
  {path: 'admin/cuisines', component: AdminCuisinesComponent, canActivate: [SystemAdminAuthGuardService]},
  {path: 'admin/restaurants', component: AdminRestaurantsComponent, canActivate: [SystemAdminAuthGuardService]},
  {path: 'admin/restaurantimport', component: AdminRestaurantImportComponent, canActivate: [SystemAdminAuthGuardService]},
  {path: 'admin/dishimport', component: AdminDishImportComponent, canActivate: [SystemAdminAuthGuardService]},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SystemAdminRoutingModule {
}
