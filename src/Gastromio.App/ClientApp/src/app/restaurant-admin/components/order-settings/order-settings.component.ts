import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {debounceTime} from "rxjs/operators";

import {RestaurantAdminFacade} from "../../restaurant-admin.facade";
import {Subscription} from "rxjs";
import {ServiceInfoModel} from "../../../shared/models/restaurant.model";

@Component({
  selector: 'app-order-settings',
  templateUrl: './order-settings.component.html',
  styleUrls: [
    './order-settings.component.css',
    '../../../../assets/css/frontend_v2.min.css',
    '../../../../assets/css/backend_v2.min.css'
  ]
})
export class OrderSettingsComponent implements OnInit, OnDestroy {

  changeSupportedOrderModeForm: FormGroup;
  changeServiceInfoForm: FormGroup;

  subscription: Subscription;

  constructor(
    private facade: RestaurantAdminFacade,
    private formBuilder: FormBuilder
  ) {
    this.changeSupportedOrderModeForm = this.formBuilder.group({
      supportedOrderMode: ['', Validators.required],
    });
    this.changeSupportedOrderModeForm.valueChanges
      .pipe(debounceTime(1000))
      .subscribe(value => {
        if (this.changeSupportedOrderModeForm.dirty && this.changeSupportedOrderModeForm.valid) {
          this.facade.changeSupportedOrderMode(value.supportedOrderMode);
        }
        this.changeSupportedOrderModeForm.markAsPristine();
      });

    this.changeServiceInfoForm = this.formBuilder.group({
      pickupEnabled: [false, [Validators.required]],
      pickupAverageTime: [null],
      pickupMinimumOrderValue: [null],
      pickupMaximumOrderValue: [null],
      deliveryEnabled: [false, [Validators.required]],
      deliveryAverageTime: [null],
      deliveryMinimumOrderValue: [null],
      deliveryMaximumOrderValue: [null],
      deliveryCosts: [null],
      reservationEnabled: [false, [Validators.required]],
      hygienicHandling: ['']
    });
    this.changeServiceInfoForm.valueChanges
      .pipe(debounceTime(1000))
      .subscribe(value => {
        if (this.changeServiceInfoForm.dirty && this.changeServiceInfoForm.valid) {
          this.facade.changeServiceInfo(new ServiceInfoModel(value));
        }
        this.changeServiceInfoForm.markAsPristine();
      });
  }

  ngOnInit(): void {
    this.subscription = this.facade.getRestaurant$().subscribe(restaurant => {
      this.changeSupportedOrderModeForm.patchValue({
        supportedOrderMode: restaurant.supportedOrderMode
      });
      this.changeSupportedOrderModeForm.markAsPristine();

      this.changeServiceInfoForm.patchValue({
        pickupEnabled: restaurant.pickupInfo?.enabled ?? false,
        pickupAverageTime: restaurant.pickupInfo?.averageTime ?? null,
        pickupMinimumOrderValue: restaurant.pickupInfo?.minimumOrderValue ?? null,
        pickupMaximumOrderValue: restaurant.pickupInfo?.maximumOrderValue ?? null,
        deliveryEnabled: restaurant.deliveryInfo?.enabled ?? false,
        deliveryAverageTime: restaurant.deliveryInfo?.averageTime ?? null,
        deliveryMinimumOrderValue: restaurant.deliveryInfo?.minimumOrderValue ?? null,
        deliveryMaximumOrderValue: restaurant.deliveryInfo?.maximumOrderValue ?? null,
        deliveryCosts: restaurant.deliveryInfo?.costs ?? null,
        reservationEnabled: restaurant.reservationInfo?.enabled ?? false,
        hygienicHandling: restaurant.hygienicHandling
      });
      this.changeServiceInfoForm.markAsPristine();
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
