import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {RouterModule} from '@angular/router';
import {ReactiveFormsModule, FormsModule} from '@angular/forms';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {HttpClientModule} from '@angular/common/http';
import {BlockUIModule} from 'ng-block-ui';

import {AddCuisineComponent} from './add-cuisine/add-cuisine.component';
import {AddDishCategoryComponent} from './add-dish-category/add-dish-category.component';
import {AddDishToCartComponent} from './add-dish-to-cart/add-dish-to-cart.component';
import {AddRestaurantComponent} from './add-restaurant/add-restaurant.component';
import {AddUserComponent} from './add-user/add-user.component';
import {AdminCuisinesComponent} from './admin-cuisines/admin-cuisines.component';
import {AdminDishImportComponent} from './admin-dish-import/admin-dish-import.component';
import {AdminMyRestaurantsComponent} from './admin-my-restaurants/admin-my-restaurants.component';
import {AdminRestaurantComponent} from './admin-restaurant/admin-restaurant.component';
import {AdminRestaurantImportComponent} from './admin-restaurant-import/admin-restaurant-import.component';
import {AdminRestaurantsComponent} from './admin-restaurants/admin-restaurants.component';
import {AdminUsersComponent} from './admin-users/admin-users.component';
import {AppComponent} from './app.component';
import {AuthService} from './auth/auth.service';
import {BottomBarComponent} from './bottom-bar/bottom-bar.component';
import {ChangeCuisineComponent} from './change-cuisine/change-cuisine.component';
import {ChangeDishCategoryComponent} from './change-dish-category/change-dish-category.component';
import {ChangeRestaurantNameComponent} from './change-restaurant-name/change-restaurant-name.component';
import {ChangeUserDetailsComponent} from './change-user-details/change-user-details.component';
import {ChangeUserPasswordComponent} from './change-user-password/change-user-password.component';
import {CheckoutComponent} from './checkout/checkout.component';
import {ClientPaginationComponent} from './pagination/client-pagination.component';
import {CuisineAdminService} from './cuisine/cuisine-admin.service';
import {CustomerInformationComponent} from './customer-information/customer-information.component';
import {DishProductInfoComponent} from './dish-productinfo/dish-productinfo.component';
import {EditDishComponent} from './edit-dish/edit-dish.component';
import {EditCartDishComponent} from './edit-cart-dish/edit-cart-dish.component';
import {GeneralErrorComponent} from './http-error-handling/general-error.component';
import {GeneralTermsAndConditionsComponent} from './general-terms-and-conditions/general-terms-and-conditions.component';
import {HttpErrorHandlingService} from './http-error-handling/http-error-handling.service';
import {ImprintComponent} from './imprint/imprint.component';
import {LoginComponent} from './login/login.component';
import {OrderHomeComponent} from './order-home/order-home.component';
import {OrderRestaurantComponent} from './order-restaurant/order-restaurant.component';
import {OrderRestaurantImprintComponent} from './order-restaurant-imprint/order-restaurant-imprint.component';
import {OrderRestaurantOpeningHoursComponent} from './order-restaurant-opening-hours/order-restaurant-opening-hours.component';
import {OrderRestaurantsComponent} from './order-restaurants/order-restaurants.component';
import {OrderService} from './order/order.service';
import {OrderSummaryComponent} from './order-summary/order-summary.component';
import {PrivacyPolicyComponent} from './privacy-policy/privacy-policy.component';
import {RemoveCuisineComponent} from './remove-cuisine/remove-cuisine.component';
import {RemoveDishCategoryComponent} from './remove-dish-category/remove-dish-category.component';
import {RemoveDishComponent} from './remove-dish/remove-dish.component';
import {RemoveRestaurantComponent} from './remove-restaurant/remove-restaurant.component';
import {RemoveUserComponent} from './remove-user/remove-user.component';
import {RestaurantAdminAuthGuardService as RestaurantAdminAuthGuard} from './auth/restaurant-admin-auth-guard.service';
import {RestaurantInformationComponent} from './restaurant-information/restaurant-information.component';
import {RestaurantRestAdminService} from './restaurant-rest-admin/restaurant-rest-admin.service';
import {RestaurantSysAdminService} from './restaurant-sys-admin/restaurant-sys-admin.service';
import {ScrollSpyDirective} from './scroll-spy.directive';
import {ServerPaginationComponent} from './pagination/server-pagination.component';
import {SystemAdminAuthGuardService as SystemAdminAuthGuard} from './auth/system-admin-auth-guard.service';
import {TopBarComponent} from './top-bar/top-bar.component';
import {UserAdminService} from './user/user-admin.service';

@NgModule({
  imports: [
    BrowserModule,
    RouterModule.forRoot([
      {path: '', component: OrderHomeComponent},
      {path: 'customer-information', component: CustomerInformationComponent},
      {path: 'restaurant-information', component: RestaurantInformationComponent},
      {path: 'general-terms-and-conditions', component: GeneralTermsAndConditionsComponent},
      {path: 'imprint', component: ImprintComponent},
      {path: 'privacy-policy', component: PrivacyPolicyComponent},
      {path: 'login', component: LoginComponent},
      {path: 'restaurants', component: OrderRestaurantsComponent},
      {path: 'restaurants/:restaurantId', component: OrderRestaurantComponent},
      {path: 'checkout', component: CheckoutComponent},
      {path: 'order-summary', component: OrderSummaryComponent},
      {path: 'admin/users', component: AdminUsersComponent, canActivate: [SystemAdminAuthGuard]},
      {path: 'admin/cuisines', component: AdminCuisinesComponent, canActivate: [SystemAdminAuthGuard]},
      {path: 'admin/restaurants', component: AdminRestaurantsComponent, canActivate: [SystemAdminAuthGuard]},
      {path: 'admin/restaurantimport', component: AdminRestaurantImportComponent, canActivate: [SystemAdminAuthGuard]},
      {path: 'admin/dishimport', component: AdminDishImportComponent, canActivate: [SystemAdminAuthGuard]},
      {path: 'admin/myrestaurants', component: AdminMyRestaurantsComponent, canActivate: [RestaurantAdminAuthGuard]},
      {
        path: 'admin/restaurants/:restaurantId',
        component: AdminRestaurantComponent,
        canActivate: [RestaurantAdminAuthGuard]
      },
    ]),
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    HttpClientModule,
    BlockUIModule.forRoot(),
  ],
  declarations: [
    AddCuisineComponent,
    AddDishCategoryComponent,
    AddDishToCartComponent,
    AddRestaurantComponent,
    AddUserComponent,
    AdminCuisinesComponent,
    AdminDishImportComponent,
    AdminMyRestaurantsComponent,
    AdminRestaurantComponent,
    AdminRestaurantImportComponent,
    AdminRestaurantsComponent,
    AdminUsersComponent,
    AppComponent,
    BottomBarComponent,
    ChangeCuisineComponent,
    ChangeDishCategoryComponent,
    ChangeRestaurantNameComponent,
    ChangeUserDetailsComponent,
    ChangeUserPasswordComponent,
    CheckoutComponent,
    ClientPaginationComponent,
    CustomerInformationComponent,
    DishProductInfoComponent,
    EditDishComponent,
    EditCartDishComponent,
    GeneralErrorComponent,
    GeneralTermsAndConditionsComponent,
    ImprintComponent,
    LoginComponent,
    OrderHomeComponent,
    OrderRestaurantComponent,
    OrderRestaurantImprintComponent,
    OrderRestaurantOpeningHoursComponent,
    OrderRestaurantsComponent,
    OrderSummaryComponent,
    PrivacyPolicyComponent,
    RemoveCuisineComponent,
    RemoveDishCategoryComponent,
    RemoveDishComponent,
    RemoveRestaurantComponent,
    RemoveUserComponent,
    RestaurantInformationComponent,
    ScrollSpyDirective,
    ServerPaginationComponent,
    TopBarComponent,
  ],
  providers: [
    AuthService,
    HttpErrorHandlingService,
    CuisineAdminService,
    OrderService,
    RestaurantAdminAuthGuard,
    RestaurantRestAdminService,
    RestaurantSysAdminService,
    SystemAdminAuthGuard,
    UserAdminService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
