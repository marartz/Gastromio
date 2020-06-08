import {Component, OnInit, OnDestroy} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {OpeningPeriodModel, RestaurantModel} from '../restaurant/restaurant.model';
import {RestaurantRestAdminService} from '../restaurant-rest-admin/restaurant-rest-admin.service';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ChangeRestaurantNameComponent} from '../change-restaurant-name/change-restaurant-name.component';
import {FormBuilder, FormGroup} from '@angular/forms';
import {PaymentMethodModel} from '../payment-method/payment-method.model';
import {Observable, of} from 'rxjs';
import {debounceTime, distinctUntilChanged, switchMap, take} from 'rxjs/operators';
import {UserModel} from '../user/user.model';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {HttpErrorResponse} from '@angular/common/http';
import {AddDishCategoryComponent} from '../add-dish-category/add-dish-category.component';
import {ChangeDishCategoryComponent} from '../change-dish-category/change-dish-category.component';
import {DishCategoryModel} from '../dish-category/dish-category.model';
import {RemoveDishCategoryComponent} from '../remove-dish-category/remove-dish-category.component';
import {DishModel} from '../dish-category/dish.model';
import {EditDishComponent} from '../edit-dish/edit-dish.component';
import {RemoveDishComponent} from '../remove-dish/remove-dish.component';
import {CuisineModel} from '../cuisine/cuisine.model';

@Component({
  selector: 'app-admin-restaurant',
  templateUrl: './admin-restaurant.component.html',
  styleUrls: ['./admin-restaurant.component.css', '../../assets/css/frontend.min.css']
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

  changeContactInfoForm: FormGroup;
  changeContactInfoError: string;

  openingPeriodVMs: OpeningPeriodViewModel[];
  addOpeningPeriodForm: FormGroup;
  startTimeError: string;
  endTimeError: string;
  addOpeningPeriodError: string;
  removeOpeningPeriodError: string;

  changePickupInfoForm: FormGroup;
  changePickupInfoError: string;

  changeDeliveryInfoForm: FormGroup;
  changeDeliveryInfoError: string;

  changeReservationInfoForm: FormGroup;
  changeReservationInfoError: string;

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
      image: ''
    });

    this.changeAddressForm = this.formBuilder.group({
      street: '',
      zipCode: '',
      city: ''
    });

    this.changeContactInfoForm = this.formBuilder.group({
      phone: '',
      fax: '',
      webSite: '',
      responsiblePerson: '',
      emailAddress: ''
    });

    this.addOpeningPeriodForm = this.formBuilder.group({
      dayOfWeek: '',
      start: '',
      end: ''
    });

    this.changePickupInfoForm = this.formBuilder.group({
      averageTime: '',
      minimumOrderValue: '',
      maximumOrderValue: '',
      hygienicHandling: ''
    });

    this.changeDeliveryInfoForm = this.formBuilder.group({
      averageTime: '',
      minimumOrderValue: '',
      maximumOrderValue: '',
      costs: '',
      hygienicHandling: ''
    });

    this.changeReservationInfoForm = this.formBuilder.group({
      hygienicHandling: ''
    });

    this.addCuisineForm = this.formBuilder.group({
      cuisineId: ''
    });

    this.addPaymentMethodForm = this.formBuilder.group({
      paymentMethodId: ''
    });
  }

  private static parseTimeValue(text: string): TimeParseResult {
    text = text.trim();

    if (text.length < 5) {
      return new TimeParseResult(false, 0);
    }

    text = text.substr(0, 5);

    if (text.substr(2, 1) !== ':') {
      return new TimeParseResult(false, 0);
    }

    const hoursText = text.substr(0, 2);
    const hours = Number(hoursText);
    if (hours === Number.NaN) {
      return new TimeParseResult(false, 0);
    }
    if (hours < 0 || hours > 23) {
      return new TimeParseResult(false, 0);
    }

    const minutesText = text.substr(3, 2);
    const minutes = Number(minutesText);
    if (minutes === Number.NaN) {
      return new TimeParseResult(false, 0);
    }
    if (minutes < 0 || minutes > 59) {
      return new TimeParseResult(false, 0);
    }

    return new TimeParseResult(true, hours * 60 + minutes);
  }

  ngOnInit() {
    this.blockUI.start('Lade Restaurantdaten...');
    this.route.paramMap.subscribe(params => {
      this.restaurantId = params.get('restaurantId');

      const getRestaurantSubscription = this.restaurantRestAdminService.getRestaurantAsync(this.restaurantId).subscribe(
        (data) => {
          getRestaurantSubscription.unsubscribe();

          this.restaurant = data;

          this.restaurant.cuisines.sort((a, b) => {
            if (a.name < b.name) {
              return -1;
            }
            if (a.name > b.name) {
              return 1;
            }
            return 0;
          });

          this.restaurant.paymentMethods.sort((a, b) => {
            if (a.name < b.name) {
              return -1;
            }
            if (a.name > b.name) {
              return 1;
            }
            return 0;
          });

          this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);

          this.imgUrl = this.restaurant.image;

          this.changeImageForm.patchValue({
            image: this.restaurant.image
          });
          this.changeImageForm.markAsPristine();

          this.changeAddressForm.patchValue({
            street: this.restaurant.address != null ? this.restaurant.address.street : '',
            zipCode: this.restaurant.address != null ? this.restaurant.address.zipCode : '',
            city: this.restaurant.address != null ? this.restaurant.address.city : '',
          });
          this.changeAddressForm.markAsPristine();

          this.changeContactInfoForm.patchValue({
            phone: this.restaurant.contactInfo != null ? this.restaurant.contactInfo.phone : '',
            fax: this.restaurant.contactInfo != null ? this.restaurant.contactInfo.fax : '',
            webSite: this.restaurant.contactInfo != null ? this.restaurant.contactInfo.webSite : '',
            responsiblePerson: this.restaurant.contactInfo != null ? this.restaurant.contactInfo.responsiblePerson : '',
            emailAddress: this.restaurant.contactInfo != null ? this.restaurant.contactInfo.emailAddress : '',
          });
          this.changeContactInfoForm.markAsPristine();

          this.changePickupInfoForm.patchValue({
            averageTime: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.averageTime : '',
            minimumOrderValue: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.minimumOrderValue : '',
            maximumOrderValue: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.maximumOrderValue : '',
            hygienicHandling: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.hygienicHandling : ''
          });
          this.changePickupInfoForm.markAsPristine();

          this.changeDeliveryInfoForm.patchValue({
            averageTime: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.averageTime : '',
            minimumOrderValue: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.minimumOrderValue : '',
            maximumOrderValue: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.maximumOrderValue : '',
            costs: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.costs : '',
            hygienicHandling: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.hygienicHandling : ''
          });
          this.changeDeliveryInfoForm.markAsPristine();

          this.changeReservationInfoForm.patchValue({
            hygienicHandling: this.restaurant.reservationInfo != null ? this.restaurant.reservationInfo.hygienicHandling : ''
          });
          this.changeReservationInfoForm.markAsPristine();

          const getDishesSubscription = this.restaurantRestAdminService.getDishesOfRestaurantAsync(this.restaurantId).subscribe(
            (dishCategories) => {
              getDishesSubscription.unsubscribe();

              this.dishCategories = dishCategories;

              if (this.dishCategories !== undefined && this.dishCategories.length > 0) {
                this.activeDishCategoryId = this.dishCategories[0].id;
              }

              const getCuisinesSubscription = this.restaurantRestAdminService.getCuisinesAsync().subscribe(
                (cuisines) => {
                  getCuisinesSubscription.unsubscribe();
                  this.availableCuisines = cuisines.filter(cuisine => this.restaurant.cuisines
                    .findIndex(en => en.id === cuisine.id) === -1);

                  const getPaymentMethodsSubscription = this.restaurantRestAdminService
                    .getPaymentMethodsAsync().subscribe(
                      (paymentMethods) => {
                        getPaymentMethodsSubscription.unsubscribe();
                        this.availablePaymentMethods = paymentMethods.filter(paymentMethod => this.restaurant
                          .paymentMethods.findIndex(en => en.id === paymentMethod.id) === -1);
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
    if (!event.target.files || !event.target.files.length) {
      return;
    }
    const reader = new FileReader();
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

  openChangeRestaurantNameForm(): void {
    const modalRef = this.modalService.open(ChangeRestaurantNameComponent);
    modalRef.componentInstance.restaurant = this.restaurant;
    modalRef.result.then((result) => {
      this.restaurant.name = result;
    }, () => {
      // TODO
    });
  }

  onSaveImage(value): void {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.changeRestaurantImageAsync(this.restaurant.id, value.image).subscribe(() => {
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
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.changeRestaurantAddressAsync(this.restaurant.id, value).subscribe(() => {
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

  onSaveContactInfo(value): void {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.changeRestaurantContactInfoAsync(this.restaurant.id, value)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.changeContactInfoError = undefined;
        this.restaurant.contactInfo = value;
        this.changeContactInfoForm.markAsPristine();
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.changeContactInfoError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onAddOpeningPeriod(value): void {
    const dayOfWeek: number = Number(value.dayOfWeek);

    const startParseResult: TimeParseResult = AdminRestaurantComponent.parseTimeValue(value.start);
    if (!startParseResult.isValid) {
      this.startTimeError = 'Geben Sie eine gültige Zeit ein';
      return;
    } else {
      this.startTimeError = undefined;
    }

    const endParseResult: TimeParseResult = AdminRestaurantComponent.parseTimeValue(value.end);
    if (!endParseResult.isValid) {
      this.endTimeError = 'Geben Sie eine gültige Zeit ein';
      return;
    } else {
      this.endTimeError = undefined;
    }

    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.addOpeningPeriodToRestaurantAsync(this.restaurant.id,
      dayOfWeek, startParseResult.value, endParseResult.value)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();

        this.addOpeningPeriodError = undefined;
        this.addOpeningPeriodForm.reset();

        const model = new OpeningPeriodModel();
        model.dayOfWeek = dayOfWeek;
        model.start = startParseResult.value;
        model.end = endParseResult.value;

        this.restaurant.openingHours.push(model);
        this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.addOpeningPeriodError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onRemoveOpeningPeriod(openingPeriod: OpeningPeriodViewModel) {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.removeOpeningPeriodFromRestaurantAsync(this.restaurant.id,
      openingPeriod.dayOfWeek, openingPeriod.startTime)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.removeOpeningPeriodError = undefined;

        const index = this.restaurant.openingHours.findIndex(elem => elem.dayOfWeek === openingPeriod.dayOfWeek
          && elem.start === openingPeriod.startTime);
        if (index > -1) {
          this.restaurant.openingHours.splice(index, 1);
          this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);
        }
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.removeOpeningPeriodError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onSavePickupInfo(value): void {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.changeRestaurantPickupInfoAsync(this.restaurant.id, value)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.changePickupInfoError = undefined;
        this.restaurant.pickupInfo = value;
        this.changePickupInfoForm.markAsPristine();
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.changePickupInfoError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onSaveDeliveryInfo(value): void {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.changeRestaurantDeliveryInfoAsync(this.restaurant.id, value)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.changeDeliveryInfoError = undefined;
        this.restaurant.deliveryInfo = value;
        this.changeDeliveryInfoForm.markAsPristine();
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.changeDeliveryInfoError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onSaveReservationInfo(value): void {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.changeRestaurantReservationInfoAsync(this.restaurant.id, value)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.changeReservationInfoError = undefined;
        this.restaurant.reservationInfo = value;
        this.changeReservationInfoForm.markAsPristine();
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.changeReservationInfoError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onAddCuisine(value): void {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.addCuisineToRestaurantAsync(this.restaurant.id,
      value.cuisineId)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.addCuisineError = undefined;
        this.addCuisineForm.reset();
        const index = this.availableCuisines.findIndex(en => en.id === value.cuisineId);
        this.restaurant.cuisines.push(this.availableCuisines[index]);
        this.availableCuisines.splice(index, 1);
        this.restaurant.cuisines.sort((a, b) => {
          if (a.name < b.name) {
            return -1;
          }
          if (a.name > b.name) {
            return 1;
          }
          return 0;
        });
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.addCuisineError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onRemoveCuisine(cuisineId: string) {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.removeCuisineFromRestaurantAsync(this.restaurant.id,
      cuisineId)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.removeCuisineError = undefined;
        const index = this.restaurant.cuisines.findIndex(en => en.id === cuisineId);
        this.availableCuisines.push(this.restaurant.cuisines[index]);
        this.restaurant.cuisines.splice(index, 1);
        this.restaurant.cuisines.sort((a, b) => {
          if (a.name < b.name) {
            return -1;
          }
          if (a.name > b.name) {
            return 1;
          }
          return 0;
        });
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.removeCuisineError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onAddPaymentMethod(value): void {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.addPaymentMethodToRestaurantAsync(this.restaurant.id,
      value.paymentMethodId)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.addPaymentMethodError = undefined;
        this.addPaymentMethodForm.reset();
        const index = this.availablePaymentMethods.findIndex(en => en.id === value.paymentMethodId);
        this.restaurant.paymentMethods.push(this.availablePaymentMethods[index]);
        this.availablePaymentMethods.splice(index, 1);
        this.restaurant.paymentMethods.sort((a, b) => {
          if (a.name < b.name) {
            return -1;
          }
          if (a.name > b.name) {
            return 1;
          }
          return 0;
        });
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.addPaymentMethodError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onRemovePaymentMethod(paymentMethodId: string) {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.removePaymentMethodFromRestaurantAsync(this.restaurant.id,
      paymentMethodId)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.removePaymentMethodError = undefined;
        const index = this.restaurant.paymentMethods.findIndex(en => en.id === paymentMethodId);
        this.availablePaymentMethods.push(this.restaurant.paymentMethods[index]);
        this.restaurant.paymentMethods.splice(index, 1);
        this.restaurant.paymentMethods.sort((a, b) => {
          if (a.name < b.name) {
            return -1;
          }
          if (a.name > b.name) {
            return 1;
          }
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
    )

  formatUser(user: UserModel): string {
    return user.email;
  }

  addSelectedUser(): void {
    if (this.userToBeAdded === undefined) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.addAdminToRestaurantAsync(this.restaurant.id,
      this.userToBeAdded.id)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.addUserError = undefined;

        if (this.restaurant.administrators.findIndex(en => en.id === this.userToBeAdded.id) > -1) {
          return;
        }

        this.restaurant.administrators.push(this.userToBeAdded);
        this.restaurant.administrators.sort((a, b) => {
          if (a.email < b.email) {
            return -1;
          }
          if (a.email > b.email) {
            return 1;
          }
          return 0;
        });
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.addUserError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  removeUser(user: UserModel): void {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.removeAdminFromRestaurantAsync(this.restaurant.id, user.id).subscribe(() => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.removeUserError = undefined;
      const index = this.restaurant.administrators.findIndex(en => en.id === user.id);
      this.restaurant.administrators.splice(index, 1);
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.removeUserError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  openAddDishCategoryForm(): void {
    const modalRef = this.modalService.open(AddDishCategoryComponent);
    modalRef.componentInstance.restaurantId = this.restaurant.id;
    if (this.dishCategories !== undefined && this.dishCategories.length > 0) {
      modalRef.componentInstance.afterCategoryId = this.dishCategories[this.dishCategories.length - 1].id;
    }
    modalRef.result.then((result: DishCategoryModel) => {
      this.dishCategories.push(result);
      this.activeDishCategoryId = result.id;
    }, () => {
      // TODO
    });
  }

  openChangeDishCategoryForm(dishCategory: DishCategoryModel): void {
    const modalRef = this.modalService.open(ChangeDishCategoryComponent);
    modalRef.componentInstance.restaurantId = this.restaurant.id;
    modalRef.componentInstance.dishCategory = dishCategory;
    modalRef.result.then((result: DishCategoryModel) => {
      dishCategory.name = result.name;
    }, () => {
      // TODO
    });
  }

  isFirstDishCategory(dishCategory: DishCategoryModel): boolean {
    const pos = this.dishCategories.findIndex(en => en.id === dishCategory.id);
    return pos === 0;
  }

  isLastDishCategory(dishCategory: DishCategoryModel): boolean {
    const pos = this.dishCategories.findIndex(en => en.id === dishCategory.id);
    return pos === this.dishCategories.length - 1;
  }

  incOrderOfDishCategory(dishCategory: DishCategoryModel): void {
    const pos = this.dishCategories.findIndex(en => en.id === dishCategory.id);
    if (pos >= this.dishCategories.length - 1) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.incOrderOfDishCategoryAsync(this.restaurant.id, dishCategory.id).subscribe(() => {
      subscription.unsubscribe();
      [this.dishCategories[pos], this.dishCategories[pos + 1]] = [this.dishCategories[pos + 1], this.dishCategories[pos]];
      this.blockUI.stop();
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  decOrderOfDishCategory(dishCategory: DishCategoryModel): void {
    const pos = this.dishCategories.findIndex(en => en.id === dishCategory.id);
    if (pos < 1) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.decOrderOfDishCategoryAsync(this.restaurant.id, dishCategory.id).subscribe(() => {
      subscription.unsubscribe();
      [this.dishCategories[pos - 1], this.dishCategories[pos]] = [this.dishCategories[pos], this.dishCategories[pos - 1]];
      this.blockUI.stop();
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  openRemoveDishCategoryForm(dishCategory: DishCategoryModel): void {
    const modalRef = this.modalService.open(RemoveDishCategoryComponent);
    modalRef.componentInstance.restaurantId = this.restaurant.id;
    modalRef.componentInstance.dishCategory = dishCategory;
    modalRef.result.then(() => {
      const index = this.dishCategories.findIndex(en => en.id === dishCategory.id);
      if (index > -1) {
        this.dishCategories.splice(index, 1);
      }
    }, () => {
      // TODO
    });
  }

  openEditDishForm(dishCategory: DishCategoryModel, dish: DishModel): void {
    const isNew = dish === undefined;

    const modalRef = this.modalService.open(EditDishComponent);
    modalRef.componentInstance.restaurantId = this.restaurant.id;
    modalRef.componentInstance.dishCategoryId = dishCategory.id;
    modalRef.componentInstance.dish = dish;
    modalRef.result.then((resultDish: DishModel) => {
      if (isNew) {
        if (dishCategory.dishes === undefined) {
          dishCategory.dishes = new Array<DishModel>();
        }
        dishCategory.dishes.push(resultDish);
      }
    }, () => {
      // TODO
    });
  }

  isFirstDish(dishCategory: DishCategoryModel, dish: DishModel): boolean {
    const pos = dishCategory.dishes.findIndex(en => en.id === dish.id);
    return pos === 0;
  }

  isLastDish(dishCategory: DishCategoryModel, dish: DishModel): boolean {
    const pos = dishCategory.dishes.findIndex(en => en.id === dish.id);
    return pos === dishCategory.dishes.length - 1;
  }

  incOrderOfDish(dishCategory: DishCategoryModel, dish: DishModel): void {
    const pos = dishCategory.dishes.findIndex(en => en.id === dish.id);
    if (pos >= dishCategory.dishes.length - 1) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.incOrderOfDishAsync(this.restaurant.id, dish.id).subscribe(() => {
      subscription.unsubscribe();
      [dishCategory.dishes[pos], dishCategory.dishes[pos + 1]] = [dishCategory.dishes[pos + 1], dishCategory.dishes[pos]];
      this.blockUI.stop();
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  decOrderOfDish(dishCategory: DishCategoryModel, dish: DishModel): void {
    const pos = dishCategory.dishes.findIndex(en => en.id === dish.id);
    if (pos < 1) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.restaurantRestAdminService.decOrderOfDishAsync(this.restaurant.id, dish.id).subscribe(() => {
      subscription.unsubscribe();
      [dishCategory.dishes[pos - 1], dishCategory.dishes[pos]] = [dishCategory.dishes[pos], dishCategory.dishes[pos - 1]];
      this.blockUI.stop();
    }, (response: HttpErrorResponse) => {
      subscription.unsubscribe();
      this.blockUI.stop();
      this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
    });
  }

  openRemoveDishForm(dishCategory: DishCategoryModel, dish: DishModel): void {
    const modalRef = this.modalService.open(RemoveDishComponent);
    modalRef.componentInstance.restaurantId = this.restaurant.id;
    modalRef.componentInstance.dishCategoryId = dishCategory.id;
    modalRef.componentInstance.dish = dish;
    modalRef.result.then(() => {
      const index = dishCategory.dishes.findIndex(en => en.id === dish.id);
      if (index > -1) {
        dishCategory.dishes.splice(index, 1);
      }
    }, () => {
      // TODO
    });
  }
}

export class OpeningPeriodViewModel {
  dayOfWeek: number;
  dayOfWeekText: string;
  startTime: number;
  startTimeText: string;
  endTime: number;
  endTimeText: string;

  public static daysOfMonth = [
    'Montag',
    'Dienstag',
    'Mittwoch',
    'Donnerstag',
    'Freitag',
    'Samstag',
    'Sonntag'
  ];

  public static vmArrayFromModels(models: OpeningPeriodModel[]): OpeningPeriodViewModel[] {
    return models.map(deliveryTime => {
      const viewModel = new OpeningPeriodViewModel();
      viewModel.dayOfWeek = deliveryTime.dayOfWeek;
      viewModel.dayOfWeekText = this.daysOfMonth[deliveryTime.dayOfWeek];
      viewModel.startTime = deliveryTime.start;
      viewModel.startTimeText = this.totalMinutesToString(deliveryTime.start);
      viewModel.endTime = deliveryTime.end;
      viewModel.endTimeText = this.totalMinutesToString(deliveryTime.end);
      return viewModel;
    }).sort((a, b) => {
      if (a.dayOfWeek < b.dayOfWeek) {
        return -1;
      } else if (a.dayOfWeek > b.dayOfWeek) {
        return +1;
      }

      if (a.startTime < b.startTime) {
        return -1;
      } else if (a.startTime > b.startTime) {
        return +1;
      }
      return 0;
    });
  }

  public static totalMinutesToString(totalMinutes: number): string {
    const hours = Math.floor(totalMinutes / 60);
    const minutes = Math.floor(totalMinutes % 60);
    return hours.toString().padStart(2, '0') + ':' + minutes.toString().padStart(2, '0');
  }
}

class TimeParseResult {
  constructor(isValid: boolean, value: number) {
    this.isValid = isValid;
    this.value = value;
  }

  isValid: boolean;
  value: number;
}
