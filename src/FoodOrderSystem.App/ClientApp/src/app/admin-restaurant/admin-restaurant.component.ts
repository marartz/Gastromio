import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RestaurantModel, AddressModel, DeliveryTimeModel } from '../restaurant/restaurant.model';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ChangeRestaurantNameComponent } from '../change-restaurant-name/change-restaurant-name.component';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-admin-restaurant',
  templateUrl: './admin-restaurant.component.html',
  styleUrls: ['./admin-restaurant.component.css']
})
export class AdminRestaurantComponent implements OnInit, OnDestroy {
  restaurantId: string;
  restaurant: RestaurantModel;

  changeAddressForm: FormGroup;
  changeContactDetailsForm: FormGroup;
  changeDeliveryDataForm: FormGroup;
  addDeliveryTimeForm: FormGroup;

  daysOfMonth = [
    "Montag",
    "Dienstag",
    "Mittwoch",
    "Donnerstag",
    "Freitag",
    "Samstag",
    "Sonntag"
  ];

  constructor(
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private restaurantRestAdminService: RestaurantRestAdminService,
  ) {
    this.restaurant = new RestaurantModel();

    this.changeAddressForm = this.formBuilder.group({
      line1: "",
      line2: "",
      zipCode: "",
      city: "",
    });

    this.changeContactDetailsForm = this.formBuilder.group({
      webSite: "",
      imprint: "",
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
  }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.restaurantId = params.get('restaurantId');

      let subscription = this.restaurantRestAdminService.getRestaurantAsync(this.restaurantId).subscribe(
        (data) => {
          subscription.unsubscribe();

          this.restaurant = data;

          this.changeAddressForm.patchValue({
            line1: this.restaurant.address != null ? this.restaurant.address.line1 : "",
            line2: this.restaurant.address != null ? this.restaurant.address.line2 : "",
            zipCode: this.restaurant.address != null ? this.restaurant.address.zipCode : "",
            city: this.restaurant.address != null ? this.restaurant.address.city : "",
          });

          this.changeContactDetailsForm.patchValue({
            webSite: this.restaurant.webSite,
            imprint: this.restaurant.imprint,
          });

          this.changeDeliveryDataForm.patchValue({
            minimumOrderValue: this.restaurant.minimumOrderValue,
            deliveryCosts: this.restaurant.deliveryCosts,
          });
        },
        (error) => {
          subscription.unsubscribe();
          // TODO
        }
      );
    });
  }

  ngOnDestroy() {
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

  onSaveAddress(value): void {
    let subscription = this.restaurantRestAdminService.changeRestaurantAddressAsync(this.restaurant.id, value).subscribe((data) => {
      subscription.unsubscribe();
      this.restaurant.address = data.address;
    }, () => {
      subscription.unsubscribe();
      // TODO
    });
  }

  onSaveContactDetails(value): void {
    let subscription = this.restaurantRestAdminService.changeRestaurantContactDetailsAsync(this.restaurant.id, value.webSite, value.imprint).subscribe((data) => {
      subscription.unsubscribe();
      this.restaurant.webSite = data.webSite;
      this.restaurant.imprint = data.imprint;
    }, () => {
      subscription.unsubscribe();
      // TODO
    });
  }

  onSaveDeliveryData(value): void {
    let subscription = this.restaurantRestAdminService.changeRestaurantDeliveryDataAsync(this.restaurant.id, value.minimumOrderValue, value.deliveryCosts).subscribe((data) => {
      subscription.unsubscribe();
      this.restaurant.minimumOrderValue = data.minimumOrderValue;
      this.restaurant.deliveryCosts = data.deliveryCosts;
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
}

export class DeliveryTimeViewModel {
  dayOfWeek: number;
  dayOfWeekText: string;
  startTime: number;
  startTimeText: string;
  endTime: number;
  endTimeText: string;
}
