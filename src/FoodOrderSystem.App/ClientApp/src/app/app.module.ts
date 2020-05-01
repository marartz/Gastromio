import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';
import { BlockUIModule } from 'ng-block-ui';

import { AppComponent } from './app.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { BottomBarComponent } from './bottom-bar/bottom-bar.component';
import { LoginComponent } from './login/login.component';
import { AdminUsersComponent } from './admin-users/admin-users.component';
import { AddUserComponent } from './add-user/add-user.component';
import { ChangeUserDetailsComponent } from './change-user-details/change-user-details.component';
import { OrderHomeComponent } from './order-home/order-home.component';
import { AuthService } from './auth/auth.service';
import { SystemAdminAuthGuardService as SystemAdminAuthGuard } from './auth/system-admin-auth-guard.service';
import { RestaurantAdminAuthGuardService as RestaurantAdminAuthGuard } from './auth/restaurant-admin-auth-guard.service';
import { UserAdminService } from './user/user-admin.service';
import { ChangeUserPasswordComponent } from './change-user-password/change-user-password.component';
import { RemoveUserComponent } from './remove-user/remove-user.component';
import { PaginationComponent } from './pagination/pagination.component';
import { AdminCuisinesComponent } from './admin-cuisines/admin-cuisines.component';
import { AdminPaymentMethodsComponent } from './admin-payment-methods/admin-payment-methods.component';
import { AddCuisineComponent } from './add-cuisine/add-cuisine.component';
import { CuisineAdminService } from './cuisine/cuisine-admin.service';
import { ChangeCuisineComponent } from './change-cuisine/change-cuisine.component';
import { RemoveCuisineComponent } from './remove-cuisine/remove-cuisine.component';
import { AddPaymentMethodComponent } from './add-payment-method/add-payment-method.component';
import { ChangePaymentMethodComponent } from './change-payment-method/change-payment-method.component';
import { RemovePaymentMethodComponent } from './remove-payment-method/remove-payment-method.component';
import { PaymentMethodAdminService } from './payment-method/payment-method-admin.service';
import { AdminRestaurantsComponent } from './admin-restaurants/admin-restaurants.component';
import { AddRestaurantComponent } from './add-restaurant/add-restaurant.component';
import { ChangeRestaurantNameComponent } from './change-restaurant-name/change-restaurant-name.component';
import { RemoveRestaurantComponent } from './remove-restaurant/remove-restaurant.component';
import { RestaurantSysAdminService } from './restaurant-sys-admin/restaurant-sys-admin.service';
import { RestaurantRestAdminService } from './restaurant-rest-admin/restaurant-rest-admin.service';
import { AdminRestaurantComponent } from './admin-restaurant/admin-restaurant.component';
import { OrderService } from './order/order.service';
import { CustomerInformationComponent } from './customer-information/customer-information.component';
import { RestaurantInformationComponent } from './restaurant-information/restaurant-information.component';
import { GeneralTermsAndConditionsComponent } from './general-terms-and-conditions/general-terms-and-conditions.component';
import { ImprintComponent } from './imprint/imprint.component';
import { PrivacyPolicyComponent } from './privacy-policy/privacy-policy.component';

@NgModule({
  imports: [
    BrowserModule,
    RouterModule.forRoot([
      { path: '', component: OrderHomeComponent },
      { path: 'customer-information', component: CustomerInformationComponent },
      { path: 'restaurant-information', component: RestaurantInformationComponent },
      { path: 'general-terms-and-conditions', component: GeneralTermsAndConditionsComponent },
      { path: 'imprint', component: ImprintComponent },
      { path: 'privacy-policy', component: PrivacyPolicyComponent },
      { path: 'admin/users', component: AdminUsersComponent, canActivate: [SystemAdminAuthGuard] },
      { path: 'admin/cuisines', component: AdminCuisinesComponent, canActivate: [SystemAdminAuthGuard] },
      { path: 'admin/paymentmethods', component: AdminPaymentMethodsComponent, canActivate: [SystemAdminAuthGuard] },
      { path: 'admin/restaurants', component: AdminRestaurantsComponent, canActivate: [SystemAdminAuthGuard] },
      { path: 'admin/restaurants/:restaurantId', component: AdminRestaurantComponent, canActivate: [RestaurantAdminAuthGuard] },
    ]),
    ReactiveFormsModule,
    FormsModule,
    NgbModule,
    HttpClientModule,
    BlockUIModule.forRoot(),
  ],
  declarations: [
    AppComponent,
    PaginationComponent,
    TopBarComponent,
    BottomBarComponent,
    LoginComponent,
    AdminUsersComponent,
    AddUserComponent,
    ChangeUserDetailsComponent,
    ChangeUserPasswordComponent,
    RemoveUserComponent,
    AdminCuisinesComponent,
    AddCuisineComponent,
    ChangeCuisineComponent,
    RemoveCuisineComponent,
    AdminPaymentMethodsComponent,
    AddPaymentMethodComponent,
    ChangePaymentMethodComponent,
    RemovePaymentMethodComponent,
    AdminRestaurantsComponent,
    AddRestaurantComponent,
    ChangeRestaurantNameComponent,
    RemoveRestaurantComponent,
    OrderHomeComponent,
    AdminRestaurantComponent,
    CustomerInformationComponent,
    RestaurantInformationComponent,
    GeneralTermsAndConditionsComponent,
    ImprintComponent,
    PrivacyPolicyComponent,
  ],
  providers: [
    AuthService,
    SystemAdminAuthGuard,
    RestaurantAdminAuthGuard,
    UserAdminService,
    CuisineAdminService,
    PaymentMethodAdminService,
    RestaurantSysAdminService,
    RestaurantRestAdminService,
    OrderService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
