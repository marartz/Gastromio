import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {NgbModule} from '@ng-bootstrap/ng-bootstrap';

import {AuthModule} from '../auth/auth.module';
import {OrderModule} from '../order/order.module';

import {AboutComponent} from './components/about/about.component';
import {BottomBarComponent} from './components/bottom-bar/bottom-bar.component';
import {ImprintComponent} from './components/imprint/imprint.component';
import {PrivacyPolicyComponent} from './components/privacy-policy/privacy-policy.component';
import {TopBarComponent} from './components/top-bar/top-bar.component';
import {CommonUiRoutingModule} from './common-ui.routing.module';

@NgModule({
  imports: [
    CommonModule,
    NgbModule,
    CommonUiRoutingModule,
    AuthModule,
    OrderModule
  ],
  declarations: [
    AboutComponent,
    BottomBarComponent,
    ImprintComponent,
    PrivacyPolicyComponent,
    TopBarComponent
  ],
  providers: [
  ],
  exports: [
    TopBarComponent,
    BottomBarComponent
  ]
})
export class CommonUiModule {
}
