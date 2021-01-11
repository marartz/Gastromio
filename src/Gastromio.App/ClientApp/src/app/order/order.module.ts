import {NgModule} from '@angular/core';
import {SharedModule} from '../shared/shared.module';
import {OrderService} from './services/order.service';
import {OrderFacade} from "./order.facade";
import {StoredCartService} from "./services/stored-cart.service";

@NgModule({
  imports: [
    SharedModule
  ],
  declarations: [
  ],
  providers: [
    OrderFacade,
    OrderService,
    StoredCartService
  ]
})
export class OrderModule {
}
