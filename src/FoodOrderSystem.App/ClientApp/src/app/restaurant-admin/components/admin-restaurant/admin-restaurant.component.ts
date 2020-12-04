import {Component, OnInit, OnDestroy} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {RestaurantAdminFacade} from "../../restaurant-admin.facade";
import {Observable} from "rxjs";
import {RestaurantModel} from "../../../shared/models/restaurant.model";

@Component({
  selector: 'app-admin-restaurant',
  templateUrl: './admin-restaurant.component.html',
  styleUrls: [
    './admin-restaurant.component.css',
    '../../../../assets/css/frontend_v2.min.css',
    '../../../../assets/css/backend_v2.min.css'
  ]
})
export class AdminRestaurantComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  public isInitialized$: Observable<boolean>;
  public initializationError$: Observable<string>;
  public restaurant$: Observable<RestaurantModel>;
  public selectedTab$: Observable<string>;
  public updateError$: Observable<string>;

  // openingPeriodVMs: OpeningPeriodViewModel[];
  // addOpeningPeriodForm: FormGroup;
  //
  // availableCuisines: CuisineModel[];
  // addCuisineForm: FormGroup;
  //
  // paymentMethods: PaymentMethodModel[];
  // paymentMethodStatus: boolean[];
  //
  // public userToBeAdded: UserModel;
  //
  // dishCategories: DishCategoryModel[];
  // activeDishCategoryId: string;

  constructor(
    private route: ActivatedRoute,
    private facade: RestaurantAdminFacade
    // private modalService: NgbModal,
    // private formBuilder: FormBuilder,
    // private restaurantRestAdminService: RestaurantRestAdminService,
    // private httpErrorHandlingService: HttpErrorHandlingService,
    // private authService: AuthService,
    // private router: Router
  ) {
    // this.addOpeningPeriodForm = this.formBuilder.group({
    //   dayOfWeek: '',
    //   start: '',
    //   end: ''
    // });
    //
    // this.addCuisineForm = this.formBuilder.group({
    //   cuisineId: ''
    // });
  }

  ngOnInit() {
    this.facade.getIsInitializing$().subscribe(isInitializing => {
      if (isInitializing) {
        this.blockUI.start('Lade Restaurantdaten...');
      } else {
        this.blockUI.stop();
      }
    });

    this.facade.getIsUpdating$().subscribe(isUpdating => {
      if (isUpdating) {
        this.blockUI.start('Speichere Restaurantdaten...');
      } else {
        this.blockUI.stop();
      }
    });

    this.isInitialized$ = this.facade.getIsInitialized$();
    this.initializationError$ = this.facade.getInitializationError$();
    this.restaurant$ = this.facade.getRestaurant$();
    this.selectedTab$ = this.facade.getSelectedTab$();
    this.updateError$ = this.facade.getUpdateError$();

    this.route.paramMap.subscribe(params => {
      const restaurantId = params.get('restaurantId');
      this.facade.initialize(restaurantId);
    });
    //
    //   this.restaurantRestAdminService.getRestaurantAsync(this.restaurantId)
    //     .pipe(take(1))
    //     .subscribe(
    //       (data) => {
    //         this.restaurant = data;
    //
    //         this.restaurant.cuisines.sort((a, b) => {
    //           if (a.name < b.name) {
    //             return -1;
    //           }
    //           if (a.name > b.name) {
    //             return 1;
    //           }
    //           return 0;
    //         });
    //
    //         if (this.hasLogo()) {
    //           this.updateLogoUrl();
    //         }
    //
    //         if (this.hasBanner()) {
    //           this.updateBannerUrl();
    //         }
    //
    //         this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);
    //
    //
    //         this.changeServiceInfoForm.patchValue({
    //           pickupEnabled: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.enabled : false,
    //           pickupAverageTime: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.averageTime : '',
    //           pickupMinimumOrderValue: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.minimumOrderValue : '',
    //           pickupMaximumOrderValue: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.maximumOrderValue : '',
    //           deliveryEnabled: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.enabled : false,
    //           deliveryAverageTime: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.averageTime : '',
    //           deliveryMinimumOrderValue: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.minimumOrderValue : '',
    //           deliveryMaximumOrderValue: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.maximumOrderValue : '',
    //           deliveryCosts: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.costs : '',
    //           reservationEnabled: this.restaurant.reservationInfo != null ? this.restaurant.reservationInfo.enabled : false,
    //           hygienicHandling: this.restaurant.hygienicHandling
    //         });
    //         this.changeServiceInfoForm.markAsPristine();
    //
    //         this.restaurantRestAdminService.getDishesOfRestaurantAsync(this.restaurantId)
    //           .pipe(take(1))
    //           .subscribe(
    //             (dishCategories) => {
    //
    //               this.dishCategories = dishCategories;
    //
    //               if (this.dishCategories !== undefined && this.dishCategories.length > 0) {
    //                 this.activeDishCategoryId = this.dishCategories[0].id;
    //               }
    //
    //               this.restaurantRestAdminService.getCuisinesAsync()
    //                 .pipe(take(1))
    //                 .subscribe(
    //                   (cuisines) => {
    //                     this.availableCuisines = cuisines.filter(cuisine => this.restaurant.cuisines
    //                       .findIndex(en => en.id === cuisine.id) === -1);
    //
    //                     this.restaurantRestAdminService.getPaymentMethodsAsync()
    //                       .pipe(take(1))
    //                       .subscribe(
    //                         (paymentMethods) => {
    //                           this.paymentMethods = paymentMethods.sort((a, b) => {
    //                             if (a.name < b.name) {
    //                               return -1;
    //                             }
    //                             if (a.name > b.name) {
    //                               return 1;
    //                             }
    //                             return 0;
    //                           });
    //                           this.paymentMethodStatus = new Array<boolean>(this.paymentMethods.length);
    //
    //                           for (let i = 0; i < this.paymentMethods.length; i++) {
    //                             this.paymentMethodStatus[i] = this.restaurant.paymentMethods
    //                               .some(en => en.id === this.paymentMethods[i].id);
    //                           }
    //
    //                           this.blockUI.stop();
    //                         },
    //                         (error: HttpErrorResponse) => {
    //                           this.blockUI.stop();
    //                           this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
    //                         }
    //                       );
    //                   },
    //                   (error: HttpErrorResponse) => {
    //                     this.blockUI.stop();
    //                     this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
    //                   }
    //                 );
    //
    //             },
    //             (error: HttpErrorResponse) => {
    //               this.blockUI.stop();
    //               this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
    //             }
    //           );
    //
    //       },
    //       (error: HttpErrorResponse) => {
    //         this.blockUI.stop();
    //         this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
    //       }
    //     );
    // });
  }

  ngOnDestroy() {
  }

  selectTab(tab: string): void {
    this.facade.selectTab(tab);
  }

//   private static parseTimeValue(text: string): TimeParseResult {
//     if (!text) {
//       return new TimeParseResult(false, 0);
//     }
//
//     text = text.trim();
//
//     if (text.length < 5) {
//       return new TimeParseResult(false, 0);
//     }
//
//     text = text.substr(0, 5);
//
//     if (text.substr(2, 1) !== ':') {
//       return new TimeParseResult(false, 0);
//     }
//
//     const hoursText = text.substr(0, 2);
//     const hours = Number(hoursText);
//     if (hours === Number.NaN) {
//       return new TimeParseResult(false, 0);
//     }
//     if (hours < 0 || hours > 23) {
//       return new TimeParseResult(false, 0);
//     }
//
//     const minutesText = text.substr(3, 2);
//     const minutes = Number(minutesText);
//     if (minutes === Number.NaN) {
//       return new TimeParseResult(false, 0);
//     }
//     if (minutes < 0 || minutes > 59) {
//       return new TimeParseResult(false, 0);
//     }
//
//     return new TimeParseResult(true, hours * 60 + minutes);
//   }
//
//
//   logout(): void {
//     this.authService.logout();
//     this.router.navigate(['']);
//   }
//
//   getRestaurantStatusText(): string {
//     if (!this.restaurant) {
//       return '';
//     }
//     return this.restaurant.isActive ? 'Aktiv' : 'Nicht aktiv';
//   }
//
//
//   onAddOpeningPeriod(value): void {
//     const dayOfWeek: number = Number(value.dayOfWeek);
//     const invalidNumberError: string = 'Geben Sie eine gültige Zeit ein';
//
//     const startParseResult: TimeParseResult = AdminRestaurantComponent.parseTimeValue(value.start);
//     if (!startParseResult.isValid) {
//       this.formError = invalidNumberError;
//       return;
//     } else {
//       this.formError = undefined;
//     }
//
//     const endParseResult: TimeParseResult = AdminRestaurantComponent.parseTimeValue(value.end);
//     if (!endParseResult.isValid) {
//       this.formError = invalidNumberError;
//       return;
//     } else {
//       this.formError = undefined;
//     }
//
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.addOpeningPeriodToRestaurantAsync(this.restaurant.id, dayOfWeek,
//       startParseResult.value, endParseResult.value)
//       .pipe(take(1))
//       .subscribe(() => {
//         this.blockUI.stop();
//
//         this.formError = undefined;
//         this.addOpeningPeriodForm.reset();
//
//         const model = new OpeningPeriodModel();
//         model.dayOfWeek = dayOfWeek;
//         model.start = startParseResult.value;
//         model.end = endParseResult.value;
//
//         this.restaurant.openingHours.push(model);
//         this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   onRemoveOpeningPeriod(openingPeriod: OpeningPeriodViewModel) {
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.removeOpeningPeriodFromRestaurantAsync(this.restaurant.id, openingPeriod.dayOfWeek,
//       openingPeriod.startTime)
//       .pipe(take(1))
//       .subscribe(() => {
//         this.blockUI.stop();
//         this.formError = undefined;
//
//         const index = this.restaurant.openingHours.findIndex(elem => elem.dayOfWeek === openingPeriod.dayOfWeek
//           && elem.start === openingPeriod.startTime);
//         if (index > -1) {
//           this.restaurant.openingHours.splice(index, 1);
//           this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);
//         }
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   onAddCuisine(value): void {
//     if (!value || !value.cuisineId)
//       return;
//
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.addCuisineToRestaurantAsync(this.restaurant.id, value.cuisineId)
//       .pipe(take(1))
//       .subscribe(() => {
//         this.blockUI.stop();
//         this.formError = undefined;
//         this.addCuisineForm.reset();
//         const index = this.availableCuisines.findIndex(en => en.id === value.cuisineId);
//         this.restaurant.cuisines.push(this.availableCuisines[index]);
//         this.availableCuisines.splice(index, 1);
//         this.restaurant.cuisines.sort((a, b) => {
//           if (a.name < b.name) {
//             return -1;
//           }
//           if (a.name > b.name) {
//             return 1;
//           }
//           return 0;
//         });
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   onRemoveCuisine(cuisineId: string) {
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.removeCuisineFromRestaurantAsync(this.restaurant.id, cuisineId)
//       .pipe(take(1))
//       .subscribe(() => {
//         this.blockUI.stop();
//         this.formError = undefined;
//         const index = this.restaurant.cuisines.findIndex(en => en.id === cuisineId);
//         this.availableCuisines.push(this.restaurant.cuisines[index]);
//         this.restaurant.cuisines.splice(index, 1);
//         this.restaurant.cuisines.sort((a, b) => {
//           if (a.name < b.name) {
//             return -1;
//           }
//           if (a.name > b.name) {
//             return 1;
//           }
//           return 0;
//         });
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   isPaymentMethodEnabled(paymentMethod: PaymentMethodModel): boolean {
//     const index = this.paymentMethods.findIndex(en => en.id === paymentMethod.id);
//     if (index < 0) {
//       return false;
//     }
//     return this.paymentMethodStatus[index];
//   }
//
//   onPaymentMethodStatusToggled(paymentMethod: PaymentMethodModel): void {
//     const index = this.paymentMethods.findIndex(en => en.id === paymentMethod.id);
//     if (index < 0) {
//       return;
//     }
//     const currentStatus = this.paymentMethodStatus[index];
//     this.blockUI.start('Verarbeite Daten...');
//     let observable: Observable<boolean>;
//     if (currentStatus) {
//       observable = this.restaurantRestAdminService.removePaymentMethodFromRestaurantAsync(this.restaurant.id,
//         paymentMethod.id);
//     } else {
//       observable = this.restaurantRestAdminService.addPaymentMethodToRestaurantAsync(this.restaurant.id,
//         paymentMethod.id);
//     }
//     observable
//       .pipe(take(1))
//       .subscribe(() => {
//         this.blockUI.stop();
//         this.generalError = undefined;
//         this.paymentMethodStatus[index] = !this.paymentMethodStatus[index];
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   searchForUser = (text: Observable<string>) =>
//     text.pipe(
//       debounceTime(200),
//       distinctUntilChanged(),
//       switchMap(term => term.length < 2 ? of([]) : this.restaurantRestAdminService.searchForUsersAsync(term)),
//       take(10)
//     )
//
//   formatUser(user: UserModel): string {
//     return user.email;
//   }
//
//   addSelectedUser(): void {
//     if (this.userToBeAdded === undefined) {
//       return;
//     }
//
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.addAdminToRestaurantAsync(this.restaurant.id, this.userToBeAdded.id)
//       .pipe(take(1))
//       .subscribe(() => {
//         this.blockUI.stop();
//         this.formError = undefined;
//
//         if (this.restaurant.administrators.findIndex(en => en.id === this.userToBeAdded.id) > -1) {
//           return;
//         }
//
//         this.restaurant.administrators.push(this.userToBeAdded);
//         this.restaurant.administrators.sort((a, b) => {
//           if (a.email < b.email) {
//             return -1;
//           }
//           if (a.email > b.email) {
//             return 1;
//           }
//           return 0;
//         });
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   removeUser(user: UserModel): void {
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.removeAdminFromRestaurantAsync(this.restaurant.id, user.id)
//       .pipe(take(1))
//       .subscribe(() => {
//         this.blockUI.stop();
//         this.formError = undefined;
//         const index = this.restaurant.administrators.findIndex(en => en.id === user.id);
//         this.restaurant.administrators.splice(index, 1);
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   openAddDishCategoryForm(): void {
//     const modalRef = this.modalService.open(AddDishCategoryComponent);
//     modalRef.componentInstance.restaurantId = this.restaurant.id;
//     if (this.dishCategories !== undefined && this.dishCategories.length > 0) {
//       modalRef.componentInstance.afterCategoryId = this.dishCategories[this.dishCategories.length - 1].id;
//     }
//     modalRef.result.then((result: DishCategoryModel) => {
//       this.dishCategories.push(result);
//       this.activeDishCategoryId = result.id;
//     }, () => {
//       // TODO
//     });
//   }
//
//   openChangeDishCategoryForm(dishCategory: DishCategoryModel): void {
//     const modalRef = this.modalService.open(ChangeDishCategoryComponent);
//     modalRef.componentInstance.restaurantId = this.restaurant.id;
//     modalRef.componentInstance.dishCategory = dishCategory;
//     modalRef.result.then((result: DishCategoryModel) => {
//       dishCategory.name = result.name;
//     }, () => {
//       // TODO
//     });
//   }
//
//   isFirstDishCategory(dishCategory: DishCategoryModel): boolean {
//     const pos = this.dishCategories.findIndex(en => en.id === dishCategory.id);
//     return pos === 0;
//   }
//
//   isLastDishCategory(dishCategory: DishCategoryModel): boolean {
//     const pos = this.dishCategories.findIndex(en => en.id === dishCategory.id);
//     return pos === this.dishCategories.length - 1;
//   }
//
//   incOrderOfDishCategory(dishCategory: DishCategoryModel): void {
//     const pos = this.dishCategories.findIndex(en => en.id === dishCategory.id);
//     if (pos >= this.dishCategories.length - 1) {
//       return;
//     }
//
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.incOrderOfDishCategoryAsync(this.restaurant.id, dishCategory.id)
//       .pipe(take(1))
//       .subscribe(() => {
//         [this.dishCategories[pos], this.dishCategories[pos + 1]] = [this.dishCategories[pos + 1], this.dishCategories[pos]];
//         this.blockUI.stop();
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   decOrderOfDishCategory(dishCategory: DishCategoryModel): void {
//     const pos = this.dishCategories.findIndex(en => en.id === dishCategory.id);
//     if (pos < 1) {
//       return;
//     }
//
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.decOrderOfDishCategoryAsync(this.restaurant.id, dishCategory.id)
//       .pipe(take(1))
//       .subscribe(() => {
//         [this.dishCategories[pos - 1], this.dishCategories[pos]] = [this.dishCategories[pos], this.dishCategories[pos - 1]];
//         this.blockUI.stop();
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   openRemoveDishCategoryForm(dishCategory: DishCategoryModel): void {
//     const modalRef = this.modalService.open(RemoveDishCategoryComponent);
//     modalRef.componentInstance.restaurantId = this.restaurant.id;
//     modalRef.componentInstance.dishCategory = dishCategory;
//     modalRef.result.then(() => {
//       const index = this.dishCategories.findIndex(en => en.id === dishCategory.id);
//       if (index > -1) {
//         this.dishCategories.splice(index, 1);
//       }
//     }, () => {
//       // TODO
//     });
//   }
//
//   openEditDishForm(dishCategory: DishCategoryModel, dish: DishModel): void {
//     const isNew = dish === undefined;
//
//     const modalRef = this.modalService.open(EditDishComponent);
//     modalRef.componentInstance.restaurantId = this.restaurant.id;
//     modalRef.componentInstance.dishCategoryId = dishCategory.id;
//     modalRef.componentInstance.dish = dish;
//     modalRef.result.then((resultDish: DishModel) => {
//       if (isNew) {
//         if (dishCategory.dishes === undefined) {
//           dishCategory.dishes = new Array<DishModel>();
//         }
//         dishCategory.dishes.push(resultDish);
//       }
//     }, () => {
//       // TODO
//     });
//   }
//
//   isFirstDish(dishCategory: DishCategoryModel, dish: DishModel): boolean {
//     const pos = dishCategory.dishes.findIndex(en => en.id === dish.id);
//     return pos === 0;
//   }
//
//   getPricesOfDish(dish: DishModel): string {
//     if (!dish.variants || dish.variants.length === 0) {
//       return '';
//     }
//
//     if (dish.variants.length === 1) {
//       return '€' + dish.variants[0].price.toLocaleString('de', {minimumFractionDigits: 2});
//     }
//
//     let result = '';
//     for (const variant of dish.variants) {
//       if (result.length > 0) {
//         result += '; ';
//       }
//       result += variant.name + ' €' + variant.price.toLocaleString('de', {minimumFractionDigits: 2});
//     }
//
//     return result;
//   }
//
//   isLastDish(dishCategory: DishCategoryModel, dish: DishModel): boolean {
//     const pos = dishCategory.dishes.findIndex(en => en.id === dish.id);
//     return pos === dishCategory.dishes.length - 1;
//   }
//
//   incOrderOfDish(dishCategory: DishCategoryModel, dish: DishModel): void {
//     const pos = dishCategory.dishes.findIndex(en => en.id === dish.id);
//     if (pos >= dishCategory.dishes.length - 1) {
//       return;
//     }
//
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.incOrderOfDishAsync(this.restaurant.id, dish.id)
//       .pipe(take(1))
//       .subscribe(() => {
//         [dishCategory.dishes[pos], dishCategory.dishes[pos + 1]] = [dishCategory.dishes[pos + 1], dishCategory.dishes[pos]];
//         this.blockUI.stop();
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   decOrderOfDish(dishCategory: DishCategoryModel, dish: DishModel): void {
//     const pos = dishCategory.dishes.findIndex(en => en.id === dish.id);
//     if (pos < 1) {
//       return;
//     }
//
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.decOrderOfDishAsync(this.restaurant.id, dish.id)
//       .pipe(take(1))
//       .subscribe(() => {
//         [dishCategory.dishes[pos - 1], dishCategory.dishes[pos]] = [dishCategory.dishes[pos], dishCategory.dishes[pos - 1]];
//         this.blockUI.stop();
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   openRemoveDishForm(dishCategory: DishCategoryModel, dish: DishModel): void {
//     const modalRef = this.modalService.open(RemoveDishComponent);
//     modalRef.componentInstance.restaurantId = this.restaurant.id;
//     modalRef.componentInstance.dishCategoryId = dishCategory.id;
//     modalRef.componentInstance.dish = dish;
//     modalRef.result.then(() => {
//       const index = dishCategory.dishes.findIndex(en => en.id === dish.id);
//       if (index > -1) {
//         dishCategory.dishes.splice(index, 1);
//       }
//     }, () => {
//       // TODO
//     });
//   }
// }
//
// export class OpeningPeriodViewModel {
//
//   public static daysOfMonth = [
//     'Montag',
//     'Dienstag',
//     'Mittwoch',
//     'Donnerstag',
//     'Freitag',
//     'Samstag',
//     'Sonntag'
//   ];
//
//   dayOfWeek: number;
//   dayOfWeekText: string;
//   startTime: number;
//   startTimeText: string;
//   endTime: number;
//   endTimeText: string;
//
//   public static vmArrayFromModels(models: OpeningPeriodModel[]): OpeningPeriodViewModel[] {
//     return models.map(deliveryTime => {
//       const viewModel = new OpeningPeriodViewModel();
//       viewModel.dayOfWeek = deliveryTime.dayOfWeek;
//       viewModel.dayOfWeekText = this.daysOfMonth[deliveryTime.dayOfWeek];
//       viewModel.startTime = deliveryTime.start;
//       viewModel.startTimeText = this.totalMinutesToString(deliveryTime.start);
//       viewModel.endTime = deliveryTime.end;
//       viewModel.endTimeText = this.totalMinutesToString(deliveryTime.end);
//       return viewModel;
//     }).sort((a, b) => {
//       if (a.dayOfWeek < b.dayOfWeek) {
//         return -1;
//       } else if (a.dayOfWeek > b.dayOfWeek) {
//         return +1;
//       }
//
//       if (a.startTime < b.startTime) {
//         return -1;
//       } else if (a.startTime > b.startTime) {
//         return +1;
//       }
//       return 0;
//     });
//   }
//
//   public static totalMinutesToString(totalMinutes: number): string {
//     const hours = Math.floor(totalMinutes / 60);
//     const minutes = Math.floor(totalMinutes % 60);
//     return hours.toString().padStart(2, '0') + ':' + minutes.toString().padStart(2, '0');
//   }
}

// class TimeParseResult {
//   constructor(isValid: boolean, value: number) {
//     this.isValid = isValid;
//     this.value = value;
//   }
//
//   isValid: boolean;
//   value: number;
// }
