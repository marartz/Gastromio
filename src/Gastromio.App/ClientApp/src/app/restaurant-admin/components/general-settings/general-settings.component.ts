import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

import {Subscription} from "rxjs";
import {debounceTime} from "rxjs/operators";

import {AddressModel, ContactInfoModel} from "../../../shared/models/restaurant.model";

import {RestaurantAdminFacade} from "../../restaurant-admin.facade";

@Component({
  selector: 'app-general-settings',
  templateUrl: './general-settings.component.html',
  styleUrls: [
    './general-settings.component.css',
    '../../../../assets/css/frontend_v2.min.css',
    '../../../../assets/css/backend_v2.min.css'
  ]
})
export class GeneralSettingsComponent implements OnInit, OnDestroy {

  changeAddressForm: FormGroup;
  changeContactInfoForm: FormGroup;

  subscription: Subscription;

  constructor(
    private facade: RestaurantAdminFacade,
    private formBuilder: FormBuilder
  ) {
    this.changeAddressForm = this.formBuilder.group({
      street: ['', [Validators.required, Validators.pattern(/^(([a-zA-ZäöüÄÖÜß]\D*)\s+\d+?\s*.*)$/)]],
      zipCode: ['', [Validators.required, Validators.pattern(/^([0]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{3}$/)]],
      city: ['', Validators.required]
    });
    this.changeAddressForm.valueChanges
      .pipe(debounceTime(1000))
      .subscribe(value => {
        if (this.changeAddressForm.dirty && this.changeAddressForm.valid) {
          this.facade.changeAddress(new AddressModel(value));
        }
        this.changeAddressForm.markAsPristine();
      });

    this.changeContactInfoForm = this.formBuilder.group({
      phone: ['', [Validators.required, Validators.pattern(/^(((((((00|\+)49[ \-/]?)|0)[1-9][0-9]{1,4})[ \-/]?)|((((00|\+)49\()|\(0)[1-9][0-9]{1,4}\)[ \-/]?))[0-9]{1,7}([ \-/]?[0-9]{1,5})?)$/)]],
      fax: ['', Validators.pattern(/^(((((((00|\+)49[ \-/]?)|0)[1-9][0-9]{1,4})[ \-/]?)|((((00|\+)49\()|\(0)[1-9][0-9]{1,4}\)[ \-/]?))[0-9]{1,7}([ \-/]?[0-9]{1,5})?)$/)],
      webSite: ['', Validators.pattern(/^(https?:\/\/){0,1}(www\.)?[-a-zäöüA-ZÄÖÜ0-9@:%._\+~#=]{1,256}\.[a-zäöüA-ZÄÖÜ0-9()]{1,6}\b([-a-zäöüA-ZÄÖÜ0-9()@:%_\+.~#?&//=]*)$/)],
      responsiblePerson: ['', Validators.required],
      emailAddress: ['', [Validators.required, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]],
      mobile: ['', Validators.pattern(/^(((((((00|\+)49[ \-/]?)|0)[1-9][0-9]{1,4})[ \-/]?)|((((00|\+)49\()|\(0)[1-9][0-9]{1,4}\)[ \-/]?))[0-9]{1,7}([ \-/]?[0-9]{1,5})?)$/)],
      orderNotificationByMobile: [false]
    });
    this.changeContactInfoForm.valueChanges
      .pipe(debounceTime(1000))
      .subscribe(value => {
        if (this.changeContactInfoForm.dirty && this.changeContactInfoForm.valid) {
          this.facade.changeContactInfo(new ContactInfoModel(value))
        }
        this.changeContactInfoForm.markAsPristine();
      });

  }

  get af() {
    return this.changeAddressForm.controls;
  }

  get cif() {
    return this.changeContactInfoForm.controls;
  }

  ngOnInit(): void {
    this.subscription = this.facade.getRestaurant$().subscribe(restaurant => {
      this.changeAddressForm.patchValue({
        street: restaurant.address?.street ?? '',
        zipCode: restaurant.address?.zipCode ?? '',
        city: restaurant.address?.city ?? '',
      });
      this.changeAddressForm.markAsPristine();

      this.changeContactInfoForm.patchValue({
        phone: restaurant.contactInfo?.phone ?? '',
        fax: restaurant.contactInfo?.fax ?? '',
        webSite: restaurant.contactInfo?.webSite ?? '',
        responsiblePerson: restaurant.contactInfo?.responsiblePerson ?? '',
        emailAddress: restaurant.contactInfo?.emailAddress ?? '',
        mobile: restaurant.contactInfo?.mobile ?? '',
        orderNotificationByMobile: restaurant.contactInfo?.orderNotificationByMobile ?? false
      });
      this.changeContactInfoForm.markAsPristine();
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

}
