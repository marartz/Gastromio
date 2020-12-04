import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import {BlockUIModule} from 'ng-block-ui';

import {RestaurantAdminRoutingModule} from './restaurant-admin.routing.module';

import {AuthModule} from '../auth/auth.module';
import {SharedModule} from '../shared/shared.module';

import {AddDishCategoryComponent} from './components/add-dish-category/add-dish-category.component';
import {AdminMyRestaurantsComponent} from './components/admin-my-restaurants/admin-my-restaurants.component';
import {AdminRestaurantComponent} from './components/admin-restaurant/admin-restaurant.component';
import {ChangeDishCategoryComponent} from './components/change-dish-category/change-dish-category.component';
import {ChangeRestaurantNameComponent} from './components/change-restaurant-name/change-restaurant-name.component';
import {CuisineSettingsComponent} from './components/cuisine-settings/cuisine-settings.component';
import {DishManagementComponent} from './components/dish-management/dish-management.component';
import {EditDishComponent} from './components/edit-dish/edit-dish.component';
import {GeneralSettingsComponent} from "./components/general-settings/general-settings.component";
import {ImageSettingsComponent} from "./components/image-settings/image-settings.component";
import {OpeningHoursSettingsComponent} from './components/opening-hours-settings/opening-hours-settings.component';
import {OrderSettingsComponent} from './components/order-settings/order-settings.component';
import {PaymentSettingsComponent} from './components/payment-settings/payment-settings.component';
import {RemoveDishComponent} from './components/remove-dish/remove-dish.component';
import {RemoveDishCategoryComponent} from './components/remove-dish-category/remove-dish-category.component';

import {RestaurantAdminFacade} from "./restaurant-admin.facade";

import {RestaurantRestAdminService} from './services/restaurant-rest-admin.service';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgbModule,
    BlockUIModule.forRoot(),
    RestaurantAdminRoutingModule,
    SharedModule,
    AuthModule
  ],
  declarations: [
    AddDishCategoryComponent,
    AdminMyRestaurantsComponent,
    AdminRestaurantComponent,
    ChangeDishCategoryComponent,
    ChangeRestaurantNameComponent,
    CuisineSettingsComponent,
    EditDishComponent,
    GeneralSettingsComponent,
    ImageSettingsComponent,
    RemoveDishComponent,
    RemoveDishCategoryComponent,
    DishManagementComponent,
    OpeningHoursSettingsComponent,
    OrderSettingsComponent,
    PaymentSettingsComponent,
  ],
  providers: [
    RestaurantAdminFacade,
    RestaurantRestAdminService,
  ]
})
export class RestaurantAdminModule {
}
