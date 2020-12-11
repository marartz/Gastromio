import { Component, OnInit } from '@angular/core';

import {Observable} from "rxjs";
import {map} from "rxjs/operators";

import {PaymentMethodModel} from "../../../shared/models/payment-method.model";

import {RestaurantAdminFacade} from "../../restaurant-admin.facade";

@Component({
  selector: 'app-payment-settings',
  templateUrl: './payment-settings.component.html',
  styleUrls: [
    './payment-settings.component.css',
    '../../../../assets/css/frontend_v2.min.css',
    '../../../../assets/css/backend_v2.min.css'
  ]
})
export class PaymentSettingsComponent implements OnInit {

  paymentMethods$: Observable<PaymentMethodModel[]>;

  constructor(
    private facade: RestaurantAdminFacade
  ) { }

  ngOnInit(): void {
    this.paymentMethods$ = this.facade.getPaymentMethods$();
  }

  isPaymentMethodEnabled$(paymentMethodId: string): Observable<boolean> {
    return this.facade.getPaymentMethodStatus$()
      .pipe(
        map(status => {
          return status[paymentMethodId];
        })
      );
  }

  onTogglePaymentMethod(paymentMethodId: string): void {
    const cashPaymentMethodId = '8DBBC822-E4FF-47B6-8CA2-68F4F0C51AA3'.toLocaleLowerCase();
    if (paymentMethodId === cashPaymentMethodId) {
      return;
    }
    this.facade.togglePaymentMethod(paymentMethodId);
  }

}
