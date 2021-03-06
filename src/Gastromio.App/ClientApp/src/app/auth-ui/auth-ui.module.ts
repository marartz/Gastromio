import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';

import {BlockUIModule} from 'ng-block-ui';

import {AuthUiRoutingModule} from './auth-ui.routing.module';

import {ChangePasswordComponent} from './components/change-password/change-password.component';
import {ForgotPasswordComponent} from './components/forgot-password/forgot-password.component';
import {LoginComponent} from './components/login/login.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    BlockUIModule.forRoot(),
    AuthUiRoutingModule
  ],
  declarations: [
    ChangePasswordComponent,
    ForgotPasswordComponent,
    LoginComponent
  ],
  providers: [
  ]
})
export class AuthUiModule {
}
