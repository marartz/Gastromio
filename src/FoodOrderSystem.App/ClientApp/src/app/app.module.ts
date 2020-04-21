import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { TopBarComponent } from './top-bar/top-bar.component';
import { BottomBarComponent } from './bottom-bar/bottom-bar.component';
import { LoginComponent } from './login/login.component';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { AdminUsersComponent } from './admin-users/admin-users.component';
import { AddUserComponent } from './add-user/add-user.component';
import { ChangeUserDetailsComponent } from './change-user-details/change-user-details.component';
import { RestaurantAdminHomeComponent } from './restaurant-admin-home/restaurant-admin-home.component';
import { OrderHomeComponent } from './order-home/order-home.component';
import { RestaurantSearchComponent } from './restaurant-search/restaurant-search.component';

import { AuthService } from './auth/auth.service';
import { SystemAdminAuthGuardService as SystemAdminAuthGuard } from './auth/system-admin-auth-guard.service';
import { RestaurantAdminAuthGuardService as RestaurantAdminAuthGuard } from './auth/restaurant-admin-auth-guard.service';
import { UserAdminService } from './user/user-admin.service';
import { ChangeUserPasswordComponent } from './change-user-password/change-user-password.component';
import { RemoveUserComponent } from './remove-user/remove-user.component';
import { PaginationComponent } from './pagination/pagination.component';

@NgModule({
  imports: [
    BrowserModule,
    RouterModule.forRoot([
      { path: '', component: OrderHomeComponent },
      { path: 'admin', component: AdminHomeComponent, canActivate: [SystemAdminAuthGuard] },
      { path: 'admin/users', component: AdminUsersComponent, canActivate: [SystemAdminAuthGuard] },
      { path: 'restaurant/admin', component: RestaurantAdminHomeComponent },
    ]),
    ReactiveFormsModule,
    NgbModule,
    HttpClientModule,
  ],
  declarations: [
    AppComponent,
    PaginationComponent,
    TopBarComponent,
    BottomBarComponent,
    LoginComponent,
    AdminHomeComponent,
    AdminUsersComponent,
    AddUserComponent,
    ChangeUserDetailsComponent,
    ChangeUserPasswordComponent,
    RemoveUserComponent,
    RestaurantAdminHomeComponent,
    OrderHomeComponent,
    RestaurantSearchComponent,
  ],
  providers: [
    AuthService,
    SystemAdminAuthGuard,
    RestaurantAdminAuthGuard,
    UserAdminService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
