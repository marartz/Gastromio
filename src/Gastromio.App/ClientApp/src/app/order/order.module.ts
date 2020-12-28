import {NgModule} from '@angular/core';
import {SharedModule} from '../shared/shared.module';
import {OrderService} from './services/order.service';
import {OrderFacade} from "./order.facade";

@NgModule({
  imports: [
    SharedModule
  ],
  declarations: [
  ],
  providers: [
    OrderFacade,
    OrderService
  ]
})
export class OrderModule {
}
