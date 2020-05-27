import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RestaurantModel, AddressModel, DeliveryTimeModel } from '../restaurant/restaurant.model';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ChangeRestaurantNameComponent } from '../change-restaurant-name/change-restaurant-name.component';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PaymentMethodModel } from '../payment-method/payment-method.model';
import { Observable, of } from 'rxjs';
import { debounceTime, distinctUntilChanged, map, switchMap, take } from 'rxjs/operators';
import { UserModel } from '../user/user.model';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';
import { AddDishCategoryComponent } from '../add-dish-category/add-dish-category.component';
import { ChangeDishCategoryComponent } from '../change-dish-category/change-dish-category.component';
import { DishCategoryModel } from '../dish-category/dish-category.model';
import { RemoveDishCategoryComponent } from '../remove-dish-category/remove-dish-category.component';
import { DishModel } from '../dish-category/dish.model';
import { EditDishComponent } from '../edit-dish/edit-dish.component';
import { RemoveDishComponent } from '../remove-dish/remove-dish.component';
import { CuisineModel } from '../cuisine/cuisine.model';

@Component({
  selector: 'app-admin-restaurant',
  templateUrl: './admin-restaurant.component.html',
  styleUrls: ['./admin-restaurant.component.css']
})
export class AdminRestaurantComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  restaurantId: string;
  restaurant: RestaurantModel;

  generalError: string;

  changeImageForm: FormGroup;
  imgUrl: any;
  changeImageError: string;

  changeAddressForm: FormGroup;
  changeAddressError: string;

  changeContactDetailsForm: FormGroup;
  changeContactDetailsError: string;

  changeDeliveryDataForm: FormGroup;
  changeDeliveryDataError: string;

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
  startTimeError: string;
  endTimeError: string;
  addDeliveryTimeError: string;
  removeDeliveryTimeError: string;

  availableCuisines: CuisineModel[];
  addCuisineForm: FormGroup;
  addCuisineError: string;
  removeCuisineError: string;

  availablePaymentMethods: PaymentMethodModel[];
  addPaymentMethodForm: FormGroup;
  addPaymentMethodError: string;
  removePaymentMethodError: string;

  public userToBeAdded: UserModel;
  addUserError: string;
  removeUserError: string;

  dishCategories: DishCategoryModel[];
  activeDishCategoryId: string; 

  constructor(
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private restaurantRestAdminService: RestaurantRestAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
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

    this.addCuisineForm = this.formBuilder.group({
      cuisineId: ""
    });

    this.addPaymentMethodForm = this.formBuilder.group({
      paymentMethodId: ""
    });
  }

  ngOnInit() {
    this.blockUI.start("Lade Restaurantdaten...");
    this.route.paramMap.subscribe(params => {
      this.restaurantId = params.get('restaurantId');

      let getRestaurantSubscription = this.restaurantRestAdminService.getRestaurantAsync(this.restaurantId).subscribe(
        (data) => {
          getRestaurantSubscription.unsubscribe();

          this.restaurant = data;

          this.restaurant.cuisines.sort((a, b) => {
            if (a.name < b.name)
              return -1;
            if (a.name > b.name)
              return 1;
            return 0;
          });

          this.restaurant.paymentMethods.sort((a, b) => {
            if (a.name < b.name)
              return -1;
            if (a.name > b.name)
              return 1;
            return 0;
          });

          this.imgUrl = this.restaurant.image;

          this.changeImageForm.patchValue({
            image: this.restaurant.image
          });
          this.changeImageForm.markAsPristine();

          this.changeAddressForm.patchValue({
            street: this.restaurant.address != null ? this.restaurant.address.street : "",
            zipCode: this.restaurant.address != null ? this.restaurant.address.zipCode : "",
            city: this.restaurant.address != null ? this.restaurant.address.city : "",
          });
          this.changeAddressForm.markAsPristine();

          this.changeContactDetailsForm.patchValue({
            phone: this.restaurant.phone,
            webSite: this.restaurant.webSite,
            imprint: this.restaurant.imprint,
            orderEmailAddress: this.restaurant.orderEmailAddress,
          });
          this.changeContactDetailsForm.markAsPristine();

          this.changeDeliveryDataForm.patchValue({
            minimumOrderValue: this.restaurant.minimumOrderValue,
            deliveryCosts: this.restaurant.deliveryCosts,
          });
          this.changeDeliveryDataForm.markAsPristine();

          let getDishesSubscription = this.restaurantRestAdminService.getDishesOfRestaurantAsync(this.restaurantId).subscribe(
            (dishCategories) => {
              getDishesSubscription.unsubscribe();

              this.dishCategories = dishCategories;

              if (this.dishCategories !== undefined && this.dishCategories.length > 0)
                this.activeDishCategoryId = this.dishCategories[0].id;

              let getCuisinesSubscription = this.restaurantRestAdminService.getCuisinesAsync().subscribe(
                (cuisines) => {
                  getCuisinesSubscription.unsubscribe();
                  this.availableCuisines = cuisines.filter(cuisine => this.restaurant.cuisines.findIndex(en => en.id === cuisine.id) == -1);

                  let getPaymentMethodsSubscription = this.restaurantRestAdminService.getPaymentMethodsAsync().subscribe(
                    (paymentMethods) => {
                      getPaymentMethodsSubscription.unsubscribe();
                      this.availablePaymentMethods = paymentMethods.filter(paymentMethod => this.restaurant.paymentMethods.findIndex(en => en.id === paymentMethod.id) == -1);
                      this.blockUI.stop();
                    },
                    (error: HttpErrorResponse) => {
                      getPaymentMethodsSubscription.unsubscribe();
                      this.blockUI.stop();
                      this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
                    }
                  );
                },
                (error: HttpErrorResponse) => {
                  getCuisinesSubscription.unsubscribe();
                  this.blockUI.stop();
                  this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
                }
              );

            },
            (error: HttpErrorResponse) => {
              getDishesSubscription.unsubscribe();
              this.blockUI.stop();
              this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
            }
          );

        },
        (error: HttpErrorResponse) => {
          getRestaurantSubscription.unsubscribe();
          this.blockUI.stop();
          this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
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
      this.changeImageForm.markAsDirty();

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
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.changeRestaurantImageAsync(this.restaurant.id, value.image).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.changeImageError = undefined;
      this.restaurant.image = value.image;
      this.changeImageForm.markAsPristine();
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.changeImageError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  onSaveAddress(value): void {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.changeRestaurantAddressAsync(this.restaurant.id, value).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.changeAddressError = undefined;
      this.restaurant.address = value;
      this.changeAddressForm.markAsPristine();
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.changeAddressError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  onSaveContactDetails(value): void {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.changeRestaurantContactDetailsAsync(this.restaurant.id, value.phone, value.webSite, value.imprint, value.orderEmailAddress).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.changeContactDetailsError = undefined;
      this.restaurant.phone = value.phone;
      this.restaurant.webSite = value.webSite;
      this.restaurant.imprint = value.imprint;
      this.restaurant.orderEmailAddress = value.orderEmailAddress;
      this.changeContactDetailsForm.markAsPristine();
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.changeContactDetailsError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  onSaveDeliveryData(value): void {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.changeRestaurantDeliveryDataAsync(this.restaurant.id, value.minimumOrderValue, value.deliveryCosts).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.changeDeliveryDataError = undefined;
      this.restaurant.minimumOrderValue = value.minimumOrderValue;
      this.restaurant.deliveryCosts = value.deliveryCosts;
      this.changeDeliveryDataForm.markAsPristine();
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.changeDeliveryDataError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
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

    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.addDeliveryTimeToRestaurantAsync(this.restaurant.id, dayOfWeek, startParseResult.value, endParseResult.value).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();

      this.addDeliveryTimeError = undefined;
      this.addDeliveryTimeForm.reset();

      var model = new DeliveryTimeModel();
      model.dayOfWeek = dayOfWeek;
      model.start = startParseResult.value;
      model.end = endParseResult.value;

      this.restaurant.deliveryTimes.push(model);
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.addDeliveryTimeError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  onRemoveDeliveryTime(deliveryTime: DeliveryTimeViewModel) {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.removeDeliveryTimeFromRestaurantAsync(this.restaurant.id, deliveryTime.dayOfWeek, deliveryTime.startTime).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.removeDeliveryTimeError = undefined;

      let index = this.restaurant.deliveryTimes.findIndex(elem => elem.dayOfWeek === deliveryTime.dayOfWeek && elem.start === deliveryTime.startTime);
      if (index > -1) {
        this.restaurant.deliveryTimes.splice(index, 1);
      }
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.removeDeliveryTimeError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  onAddCuisine(value): void {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.addCuisineToRestaurantAsync(this.restaurant.id, value.cuisineId).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.addCuisineError = undefined;
      this.addCuisineForm.reset();
      let index = this.availableCuisines.findIndex(en => en.id === value.cuisineId);
      this.restaurant.cuisines.push(this.availableCuisines[index]);
      this.availableCuisines.splice(index, 1);
      this.restaurant.cuisines.sort((a, b) => {
        if (a.name < b.name)
          return -1;
        if (a.name > b.name)
          return 1;
        return 0;
      });
    }, (response: HttpErrorResponse) => {
      console.log("Response: ", response);
      subscription.unsubscribe();
      this.blockUI.stop();
      this.addCuisineError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  onRemoveCuisine(cuisineId: string) {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.removeCuisineFromRestaurantAsync(this.restaurant.id, cuisineId).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.removeCuisineError = undefined;
      let index = this.restaurant.cuisines.findIndex(en => en.id == cuisineId);
      this.availableCuisines.push(this.restaurant.cuisines[index]);
      this.restaurant.cuisines.splice(index, 1);
      this.restaurant.cuisines.sort((a, b) => {
        if (a.name < b.name)
          return -1;
        if (a.name > b.name)
          return 1;
        return 0;
      });
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.removeCuisineError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  onAddPaymentMethod(value): void {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.addPaymentMethodToRestaurantAsync(this.restaurant.id, value.paymentMethodId).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.addPaymentMethodError = undefined;
      this.addPaymentMethodForm.reset();
      let index = this.availablePaymentMethods.findIndex(en => en.id === value.paymentMethodId);
      this.restaurant.paymentMethods.push(this.availablePaymentMethods[index]);
      this.availablePaymentMethods.splice(index, 1);
      this.restaurant.paymentMethods.sort((a, b) => {
        if (a.name < b.name)
          return -1;
        if (a.name > b.name)
          return 1;
        return 0;
      });
    }, (response: HttpErrorResponse) => {
        console.log("Response: ", response);
      subscription.unsubscribe();
      this.blockUI.stop();
      this.addPaymentMethodError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  onRemovePaymentMethod(paymentMethodId: string) {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.removePaymentMethodFromRestaurantAsync(this.restaurant.id, paymentMethodId).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.removePaymentMethodError = undefined;
      let index = this.restaurant.paymentMethods.findIndex(en => en.id == paymentMethodId);
      this.availablePaymentMethods.push(this.restaurant.paymentMethods[index]);
      this.restaurant.paymentMethods.splice(index, 1);
      this.restaurant.paymentMethods.sort((a, b) => {
        if (a.name < b.name)
          return -1;
        if (a.name > b.name)
          return 1;
        return 0;
      });
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.removePaymentMethodError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
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

    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.addAdminToRestaurantAsync(this.restaurant.id, this.userToBeAdded.id).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.addUserError = undefined;

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
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.addUserError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  removeUser(user: UserModel): void {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantRestAdminService.removeAdminFromRestaurantAsync(this.restaurant.id, user.id).subscribe((data) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.removeUserError = undefined;
      let index = this.restaurant.administrators.findIndex(en => en.id === user.id);
      this.restaurant.administrators.splice(index, 1);
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.removeUserError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  openAddDishCategoryForm(): void {
    let modalRef = this.modalService.open(AddDishCategoryComponent);
    modalRef.componentInstance.restaurantId = this.restaurant.id;
    modalRef.result.then((result: DishCategoryModel) => {
      this.dishCategories.push(result);
      this.activeDishCategoryId = result.id;
    }, () => {
      // TODO
    });
  }

  openChangeDishCategoryForm(dishCategory: DishCategoryModel): void {
    let modalRef = this.modalService.open(ChangeDishCategoryComponent);
    modalRef.componentInstance.restaurantId = this.restaurant.id;
    modalRef.componentInstance.dishCategory = dishCategory;
    modalRef.result.then((result: DishCategoryModel) => {
      dishCategory.name = result.name;
    }, () => {
      // TODO
    });
  }

  openRemoveDishCategoryForm(dishCategory: DishCategoryModel): void {
    let modalRef = this.modalService.open(RemoveDishCategoryComponent);
    modalRef.componentInstance.restaurantId = this.restaurant.id;
    modalRef.componentInstance.dishCategory = dishCategory;
    modalRef.result.then((result) => {
      let index = this.dishCategories.findIndex(en => en.id == dishCategory.id);
      if (index > -1)
        this.dishCategories.splice(index, 1);
    }, () => {
      // TODO
    });
  }

  openEditDishForm(dishCategory: DishCategoryModel, dish: DishModel): void {
    let isNew = dish === undefined;

    let modalRef = this.modalService.open(EditDishComponent);
    modalRef.componentInstance.restaurantId = this.restaurant.id;
    modalRef.componentInstance.dishCategoryId = dishCategory.id;
    modalRef.componentInstance.dish = dish;
    modalRef.result.then((resultDish: DishModel) => {
      if (isNew) {
        if (dishCategory.dishes === undefined)
          dishCategory.dishes = new Array<DishModel>();
        dishCategory.dishes.push(resultDish);
      }
    }, () => {
      // TODO
    });
  }

  openRemoveDishForm(dishCategory: DishCategoryModel, dish: DishModel): void {
    let modalRef = this.modalService.open(RemoveDishComponent);
    modalRef.componentInstance.restaurantId = this.restaurant.id;
    modalRef.componentInstance.dishCategoryId = dishCategory.id;
    modalRef.componentInstance.dish = dish;
    modalRef.result.then((result) => {
      let index = dishCategory.dishes.findIndex(en => en.id == dish.id);
      if (index > -1)
        dishCategory.dishes.splice(index, 1);
    }, () => {
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
