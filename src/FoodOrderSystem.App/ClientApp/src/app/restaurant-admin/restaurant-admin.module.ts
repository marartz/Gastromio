import {NgModule} from '@angular/core';
import {CommonModule} from "@angular/common";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

import {NgbModule} from "@ng-bootstrap/ng-bootstrap";

import {BlockUIModule} from "ng-block-ui";

import {RestaurantAdminRoutingModule} from "./restaurant-admin.routing.module";

import {AuthModule} from "../auth/auth.module";
import {SharedModule} from "../shared/shared.module";

import {RestaurantRestAdminService} from "./services/restaurant-rest-admin.service";
import {AddDishCategoryComponent} from "./components/add-dish-category/add-dish-category.component";
import {AdminMyRestaurantsComponent} from "./components/admin-my-restaurants/admin-my-restaurants.component";
import {AdminRestaurantComponent} from "./components/admin-restaurant/admin-restaurant.component";
import {ChangeDishCategoryComponent} from "./components/change-dish-category/change-dish-category.component";
import {ChangeRestaurantNameComponent} from "./components/change-restaurant-name/change-restaurant-name.component";
import {EditDishComponent} from "./components/edit-dish/edit-dish.component";
import {RemoveDishComponent} from "./components/remove-dish/remove-dish.component";
import {RemoveDishCategoryComponent} from "./components/remove-dish-category/remove-dish-category.component";

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
    EditDishComponent,
    RemoveDishComponent,
    RemoveDishCategoryComponent
  ],
  providers: [
    RestaurantRestAdminService,
  ]
})
export class RestaurantAdminModule {
}
