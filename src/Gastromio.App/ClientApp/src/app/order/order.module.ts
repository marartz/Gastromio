import {NgModule} from '@angular/core';
import {SharedModule} from '../shared/shared.module';
import {OrderService} from './services/order.service';

@NgModule({
  imports: [
    SharedModule
  ],
  declarations: [
  ],
  providers: [
    OrderService
  ]
})
export class OrderModule {
}
