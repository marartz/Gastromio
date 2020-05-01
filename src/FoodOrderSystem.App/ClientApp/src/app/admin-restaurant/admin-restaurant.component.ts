import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RestaurantModel, AddressModel, DeliveryTimeModel } from '../restaurant/restaurant.model';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChangeRestaurantNameComponent } from '../change-restaurant-name/change-restaurant-name.component';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PaymentMethodModel } from '../payment-method/payment-method.model';
import { Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap, take } from 'rxjs/operators';
import { UserModel } from '../user/user.model';

@Component({
  selector: 'app-admin-restaurant',
  templateUrl: './admin-restaurant.component.html',
  styleUrls: ['./admin-restaurant.component.css']
})
export class AdminRestaurantComponent implements OnInit, OnDestroy {
  restaurantId: string;

  restaurant: RestaurantModel;

  paymentMethods: PaymentMethodModel[];

  changeImageForm: FormGroup;
  changeAddressForm: FormGroup;
  changeContactDetailsForm: FormGroup;
  changeDeliveryDataForm: FormGroup;
  addDeliveryTimeForm: FormGroup;
  addPaymentMethodForm: FormGroup;

  imgUrl: any;

  daysOfMonth = [
    "Montag",
    "Dienstag",
    "Mittwoch",
    "Donnerstag",
    "Freitag",
    "Samstag",
    "Sonntag"
  ];

  startTimeError: string;
  endTimeError: string;

  public userToBeAdded: UserModel;

  constructor(
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private restaurantRestAdminService: RestaurantRestAdminService,
  ) {
    this.restaurant = new RestaurantModel();

    this.changeImageForm = this.formBuilder.group({
      image: ""
    });

    this.changeAddressForm = this.formBuilder.group({
      street: "",
      zipCode: "",
      city: "",
    });

    this.changeContactDetailsForm = this.formBuilder.group({
      phone: "",
      webSite: "",
      imprint: "",
      orderEmailAddress: "",
    });

    this.changeDeliveryDataForm = this.formBuilder.group({
      minimumOrderValue: 0,
      deliveryCosts: 0,
    });

    this.addDeliveryTimeForm = this.formBuilder.group({
      dayOfWeek: "",
      start: "",
      end: ""
    });

    this.addPaymentMethodForm = this.formBuilder.group({
      paymentMethodId: ""
    });
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.restaurantId = params.get('restaurantId');

      let getRestaurantSubscription = this.restaurantRestAdminService.getRestaurantAsync(this.restaurantId).subscribe(
        (data) => {
          getRestaurantSubscription.unsubscribe();

          this.restaurant = data;

          this.imgUrl = this.restaurant.image;

          this.changeImageForm.patchValue({
            image: this.restaurant.image
          });

          this.changeAddressForm.patchValue({
            street: this.restaurant.address != null ? this.restaurant.address.street : "",
            zipCode: this.restaurant.address != null ? this.restaurant.address.zipCode : "",
            city: this.restaurant.address != null ? this.restaurant.address.city : "",
          });

          this.changeContactDetailsForm.patchValue({
            phone: this.restaurant.phone,
            webSite: this.restaurant.webSite,
            imprint: this.restaurant.imprint,
            orderEmailAddress: this.restaurant.orderEmailAddress,
          });

          this.changeDeliveryDataForm.patchValue({
            minimumOrderValue: this.restaurant.minimumOrderValue,
            deliveryCosts: this.restaurant.deliveryCosts,
          });
        },
        (error) => {
          getRestaurantSubscription.unsubscribe();
          // TODO
        }
      );

      let getPaymentMethodsSubscription = this.restaurantRestAdminService.getPaymentMethodsAsync().subscribe(
        (data) => {
          getPaymentMethodsSubscription.unsubscribe();
          this.paymentMethods = data;
        },
        (error) => {
          getPaymentMethodsSubscription.unsubscribe();
          // TODO
        }
      );
    });
  }

  ngOnDestroy() {
  }

  onImageChange(event) {
    if (!event.target.files || !event.target.files.length)
      return;
    let reader = new FileReader();
    const [file] = event.target.files;
    reader.readAsDataURL(file);

    reader.onload = () => {
      this.changeImageForm.patchValue({
        image: reader.result
      });

      this.imgUrl = reader.result;
    };
  }

  getDeliveryTimeViewModels(): Array<DeliveryTimeViewModel> {
    let tempDeliveryTimes = this.restaurant.deliveryTimes.sort((a, b) => {
      if (a.dayOfWeek < b.dayOfWeek)
        return -1;
      else if (a.dayOfWeek > b.dayOfWeek)
        return +1;

      if (a.start < b.start)
        return -1;
      else if (a.start > b.start)
        return +1;
    });

    let result = new Array<DeliveryTimeViewModel>();
    for (let deliveryTime of tempDeliveryTimes) {
      let viewModel = new DeliveryTimeViewModel();
      viewModel.dayOfWeek = deliveryTime.dayOfWeek;
      viewModel.dayOfWeekText = this.daysOfMonth[deliveryTime.dayOfWeek];
      viewModel.startTime = deliveryTime.start;
      viewModel.startTimeText = this.toTimeViewModel(deliveryTime.start);
      viewModel.endTime = deliveryTime.end;
      viewModel.endTimeText = this.toTimeViewModel(deliveryTime.end);
      result.push(viewModel);
    }

    return result;
  }

  openChangeRestaurantNameForm(): void {
    let modalRef = this.modalService.open(ChangeRestaurantNameComponent);
    modalRef.componentInstance.restaurant = this.restaurant;
    modalRef.result.then((result) => {
      this.restaurant.name = result;
    }, () => {
      // TODO
    });
  }

  onSaveImage(value): void {
    let subscription = this.restaurantRestAdminService.changeRestaurantImageAsync(this.restaurant.id, value.image).subscribe((data) => {
      subscription.unsubscribe();
      this.restaurant.image = value.image;
    }, () => {
      subscription.unsubscribe();
      // TODO
    });
  }

  onSaveAddress(value): void {
    let subscription = this.restaurantRestAdminService.changeRestaurantAddressAsync(this.restaurant.id, value).subscribe((data) => {
      subscription.unsubscribe();
      this.restaurant.address = value;
    }, () => {
      subscription.unsubscribe();
      // TODO
    });
  }

  onSaveContactDetails(value): void {
    let subscription = this.restaurantRestAdminService.changeRestaurantContactDetailsAsync(this.restaurant.id, value.phone, value.webSite, value.imprint, value.orderEmailAddress).subscribe((data) => {
      subscription.unsubscribe();
      this.restaurant.phone = value.phone;
      this.restaurant.webSite = value.webSite;
      this.restaurant.imprint = value.imprint;
      this.restaurant.orderEmailAddress = value.orderEmailAddress;
    }, () => {
      subscription.unsubscribe();
      // TODO
    });
  }

  onSaveDeliveryData(value): void {
    let subscription = this.restaurantRestAdminService.changeRestaurantDeliveryDataAsync(this.restaurant.id, value.minimumOrderValue, value.deliveryCosts).subscribe((data) => {
      subscription.unsubscribe();
      this.restaurant.minimumOrderValue = value.minimumOrderValue;
      this.restaurant.deliveryCosts = value.deliveryCosts;
    }, () => {
      subscription.unsubscribe();
      // TODO
    });
  }

  onAddDeliveryTime(value): void {
    let dayOfWeek: number = Number(value.dayOfWeek);

    let startParseResult: TimeParseResult = this.parseTimeValue(value.start);
    if (!startParseResult.isValid) {
      this.startTimeError = "Geben Sie eine gültige Zeit ein";
      return;
    } else {
      this.startTimeError = undefined;
    }

    let endParseResult: TimeParseResult = this.parseTimeValue(value.end);
    if (!endParseResult.isValid) {
      this.endTimeError = "Geben Sie eine gültige Zeit ein";
      return;
    } else {
      this.endTimeError = undefined;
    }

    let subscription = this.restaurantRestAdminService.addDeliveryTimeToRestaurantAsync(this.restaurant.id, dayOfWeek, startParseResult.value, endParseResult.value).subscribe((data) => {
      subscription.unsubscribe();
      this.addDeliveryTimeForm.reset();

      var model = new DeliveryTimeModel();
      model.dayOfWeek = dayOfWeek;
      model.start = startParseResult.value;
      model.end = endParseResult.value;

      this.restaurant.deliveryTimes.push(model);
    }, () => {
      subscription.unsubscribe();
      // TODO
    });
  }

  onRemoveDeliveryTime(deliveryTime: DeliveryTimeViewModel) {
    let subscription = this.restaurantRestAdminService.removeDeliveryTimeFromRestaurantAsync(this.restaurant.id, deliveryTime.dayOfWeek, deliveryTime.startTime).subscribe((data) => {
      subscription.unsubscribe();

      let index = this.restaurant.deliveryTimes.findIndex(elem => elem.dayOfWeek === deliveryTime.dayOfWeek && elem.start === deliveryTime.startTime);
      if (index > -1) {
        this.restaurant.deliveryTimes.splice(index, 1);
      }
    }, () => {
      subscription.unsubscribe();
      // TODO
    });
  }

  searchForUser = (text: Observable<string>) =>
    text.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => term.length < 2 ? of([]) : this.restaurantRestAdminService.searchForUsersAsync(term)),
      take(10)
    );

  formatUser(user: UserModel): string {
    let result = user.name;

    if (user.email !== undefined && user.email !== null) {
      result = result + " (" + user.email + ")";
    }

    return result;
  }

  addSelectedUser(): void {
    if (this.userToBeAdded === undefined)
      return;

    let subscription = this.restaurantRestAdminService.addAdminToRestaurantAsync(this.restaurant.id, this.userToBeAdded.id).subscribe((data) => {
      subscription.unsubscribe();

      if (this.restaurant.administrators.findIndex(en => en.id === this.userToBeAdded.id) > -1)
        return;

      this.restaurant.administrators.push(this.userToBeAdded);
      this.restaurant.administrators.sort((a, b) => {
        if (a.name < b.name)
          return -1;
        if (a.name > b.name)
          return 1;
        return 0;
      });
    }, () => {
      subscription.unsubscribe();
      // TODO
    });
  }

  removeUser(user: UserModel): void {
    let subscription = this.restaurantRestAdminService.removeAdminFromRestaurantAsync(this.restaurant.id, user.id).subscribe((data) => {
      subscription.unsubscribe();
      let index = this.restaurant.administrators.findIndex(en => en.id === user.id);
      this.restaurant.administrators.splice(index, 1);
    }, () => {
      subscription.unsubscribe();
      // TODO
    });
  }

  private toTimeViewModel(totalMinutes: number): string {
    let hours = Math.floor(totalMinutes / 60);
    let minutes = Math.floor(totalMinutes % 60);
    return hours.toString().padStart(2, "0") + ":" + minutes.toString().padStart(2, "0");
  }

  private parseTimeValue(text: string): TimeParseResult {
    text = text.trim();

    if (text.length < 5)
      return new TimeParseResult(false, 0);

    text = text.substr(0, 5);

    if (text.substr(2, 1) !== ":")
      return new TimeParseResult(false, 0);

    let hoursText = text.substr(0, 2);
    let hours = Number(hoursText);
    if (hours === Number.NaN)
      return new TimeParseResult(false, 0);
    if (hours < 0 || hours > 23)
      return new TimeParseResult(false, 0);

    let minutesText = text.substr(3, 2);
    let minutes = Number(minutesText);
    if (minutes === Number.NaN)
      return new TimeParseResult(false, 0);
    if (minutes < 0 || minutes > 59)
      return new TimeParseResult(false, 0);

    return new TimeParseResult(true, hours * 60 + minutes);
  }
}

export class DeliveryTimeViewModel {
  dayOfWeek: number;
  dayOfWeekText: string;
  startTime: number;
  startTimeText: string;
  endTime: number;
  endTimeText: string;
}

class TimeParseResult {
  constructor(isValid: boolean, value: number) {
    this.isValid = isValid;
    this.value = value;
  }

  isValid: boolean;
  value: number;
}
