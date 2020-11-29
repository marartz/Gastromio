import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RouterModule} from '@angular/router';

import {CommonUiModule} from "./common-ui/common-ui.module";
import {AuthUiModule} from "./auth-ui/auth-ui.module";
import {OrderUiModule} from "./order-ui/order-ui.module";

import {AppComponent} from './app.component';

@NgModule({
  imports: [
    BrowserModule,
    CommonUiModule,
    AuthUiModule,
    OrderUiModule,
    RouterModule.forRoot([
      // {path: 'admin/users', component: AdminUsersComponent, canActivate: [SystemAdminAuthGuard]},
      // {path: 'admin/cuisines', component: AdminCuisinesComponent, canActivate: [SystemAdminAuthGuard]},
      // {path: 'admin/restaurants', component: AdminRestaurantsComponent, canActivate: [SystemAdminAuthGuard]},
      // {path: 'admin/restaurantimport', component: AdminRestaurantImportComponent, canActivate: [SystemAdminAuthGuard]},
      // {path: 'admin/dishimport', component: AdminDishImportComponent, canActivate: [SystemAdminAuthGuard]},
      // {path: 'admin/myrestaurants', component: AdminMyRestaurantsComponent, canActivate: [RestaurantAdminAuthGuard]},
      // {path: 'admin/restaurants/:restaurantId', component: AdminRestaurantComponent, canActivate: [RestaurantAdminAuthGuard]},
    ]),
  ],
  declarations: [
    AppComponent
  ],
  providers: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
