import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';

import { CommonUiModule } from './common-ui/common-ui.module';
import { AuthUiModule } from './auth-ui/auth-ui.module';
import { OrderUiModule } from './order-ui/order-ui.module';
import { SystemAdminModule } from './system-admin/system-admin.module';
import { RestaurantAdminModule } from './restaurant-admin/restaurant-admin.module';

import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  imports: [
    BrowserModule,
    CommonUiModule,
    AuthUiModule,
    OrderUiModule,
    SystemAdminModule,
    RestaurantAdminModule,
    RouterModule.forRoot([]),
    NgbModule
  ],
  declarations: [AppComponent],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
