import {Injectable} from "@angular/core";
import {HttpErrorResponse} from "@angular/common/http";

import {BehaviorSubject, Observable} from "rxjs";
import {map, take} from "rxjs/operators";

import {
  AddressModel,
  ContactInfoModel, DeliveryInfoModel,
  PickupInfoModel, ReservationInfoModel,
  RestaurantModel,
  ServiceInfoModel
} from "../shared/models/restaurant.model";

import {RestaurantRestAdminService} from "./services/restaurant-rest-admin.service";
import {HttpErrorHandlingService} from "../shared/services/http-error-handling.service";

@Injectable()
export class RestaurantAdminFacade {

  private restaurantId: string;
  private isInitializing$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  private isInitialized$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(undefined);
  private initializationError$: BehaviorSubject<string> = new BehaviorSubject<string>(undefined);
  private restaurant$: BehaviorSubject<RestaurantModel> = new BehaviorSubject<RestaurantModel>(undefined);
  private selectedTab$: BehaviorSubject<string> = new BehaviorSubject<string>("general");
  private isUpdating$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private updateError$: BehaviorSubject<string> = new BehaviorSubject<string>(undefined);

  constructor(
    private restaurantAdminService: RestaurantRestAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  public initialize(restaurantId: string): void {
    this.restaurantId = restaurantId;

    this.restaurantAdminService.getRestaurantAsync(restaurantId)
      .pipe(take(1))
      .subscribe(
        (data) => {
          this.restaurant$.next(data);

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
          //         this.changeAddressForm.patchValue({
          //           street: this.restaurant.address != null ? this.restaurant.address.street : '',
          //           zipCode: this.restaurant.address != null ? this.restaurant.address.zipCode : '',
          //           city: this.restaurant.address != null ? this.restaurant.address.city : '',
          //         });
          //         this.changeAddressForm.markAsPristine();
          //
          //         this.changeContactInfoForm.patchValue({
          //           phone: this.restaurant.contactInfo != null ? this.restaurant.contactInfo.phone : '',
          //           fax: this.restaurant.contactInfo != null ? this.restaurant.contactInfo.fax : '',
          //           webSite: this.restaurant.contactInfo != null ? this.restaurant.contactInfo.webSite : '',
          //           responsiblePerson: this.restaurant.contactInfo != null ? this.restaurant.contactInfo.responsiblePerson : '',
          //           emailAddress: this.restaurant.contactInfo != null ? this.restaurant.contactInfo.emailAddress : '',
          //         });
          //         this.changeContactInfoForm.markAsPristine();
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


          this.isInitializing$.next(false);
          this.isInitialized$.next(true);
        },
        (error: HttpErrorResponse) => {
          this.isInitializing$.next(false);
          this.isInitialized$.next(false);
          this.initializationError$.next(this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors());
        }
      );
  }

  public getIsInitializing$(): Observable<boolean> {
    return this.isInitializing$;
  }

  public getIsInitialized$(): Observable<boolean> {
    return this.isInitialized$;
  }

  public getInitializationError$(): Observable<string> {
    return this.initializationError$;
  }

  public getRestaurant$(): Observable<RestaurantModel> {
    return this.restaurant$;
  }

  public getSelectedTab$(): Observable<string> {
    return this.selectedTab$;
  }

  public getIsUpdating$(): Observable<boolean> {
    return this.isUpdating$;
  }

  public getUpdateError$(): Observable<string> {
    return this.updateError$;
  }

  public getHasLogo$(): Observable<boolean> {
    return this.restaurant$.asObservable()
      .pipe(
        map(restaurant => {
          if (!restaurant || !restaurant.imageTypes) {
            return false;
          }
          return restaurant.imageTypes.some(en => en === 'logo');
        })
      );
  }

  public getLogoUrl$(): Observable<string> {
    return this.restaurant$.asObservable()
      .pipe(
        map(restaurant => {
          if (restaurant && restaurant.imageTypes && restaurant.imageTypes.some(en => en === 'logo')) {
            return '/api/v1/restaurants/' + restaurant.id + '/images/logo?random=' + Math.random();
          } else {
            return undefined;
          }
        })
      );
  }

  public getHasBanner$(): Observable<boolean> {
    return this.restaurant$.asObservable()
      .pipe(
        map(restaurant => {
          if (!restaurant || !restaurant.imageTypes) {
            return false;
          }
          return restaurant.imageTypes.some(en => en === 'banner');
        })
      );
  }

  public getBannerUrl$(): Observable<string> {
    return this.restaurant$.asObservable()
      .pipe(
        map(restaurant => {
          if (restaurant && restaurant.imageTypes && restaurant.imageTypes.some(en => en === 'banner')) {
            return '/api/v1/restaurants/' + restaurant.id + '/images/banner?random=' + Math.random();
          } else {
            return undefined;
          }
        })
      );
  }


  public selectTab(tab: string): void {
    this.selectedTab$.next(tab);
  }

  changeAddress(address: AddressModel): void {
    this.isUpdating$.next(true);
    this.restaurantAdminService.changeRestaurantAddressAsync(this.restaurant$.value.id, address)
      .pipe(take(1))
      .subscribe(() => {
        this.isUpdating$.next(false);
        this.updateError$.next(undefined);

        this.restaurant$.value.address = address;

        this.restaurant$.next(this.restaurant$.value)
      }, response => {
        this.isUpdating$.next(false);
        this.updateError$.next(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

  changeContactInfo(contactInfo: ContactInfoModel): void {
    this.isUpdating$.next(true);
    this.restaurantAdminService.changeRestaurantContactInfoAsync(this.restaurant$.value.id, contactInfo)
      .pipe(take(1))
      .subscribe(() => {
        this.isUpdating$.next(false);
        this.updateError$.next(undefined);
        this.restaurant$.value.contactInfo = contactInfo;
        this.restaurant$.next(this.restaurant$.value)
      }, response => {
        this.isUpdating$.next(false);
        this.updateError$.next(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

  changeLogo(logo: string): void {
    this.isUpdating$.next(true);
    this.restaurantAdminService.changeRestaurantImageAsync(this.restaurant$.value.id, 'logo', logo)
      .pipe(take(1))
      .subscribe(() => {
        this.isUpdating$.next(false);
        this.updateError$.next(undefined);

        if (!this.restaurant$.value.imageTypes) {
          this.restaurant$.value.imageTypes = new Array<string>();
        }
        if (!this.restaurant$.value.imageTypes.some(en => en === 'logo')) {
          this.restaurant$.value.imageTypes.push('logo');
        }

        this.restaurant$.next(this.restaurant$.value)
      }, (response: HttpErrorResponse) => {
        this.isUpdating$.next(false);
        this.updateError$.next(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

  removeLogo(): void {
    this.isUpdating$.next(true);
    this.restaurantAdminService.changeRestaurantImageAsync(this.restaurant$.value.id, 'logo', undefined)
      .pipe(take(1))
      .subscribe(() => {
        this.isUpdating$.next(false);
        this.updateError$.next(undefined);

        this.restaurant$.value.imageTypes = this.restaurant$.value.imageTypes.filter(en => en !== 'logo');

        this.restaurant$.next(this.restaurant$.value)
      }, (response: HttpErrorResponse) => {
        this.isUpdating$.next(false);
        this.updateError$.next(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

  changeBanner(banner: string): void {
    this.isUpdating$.next(true);
    this.restaurantAdminService.changeRestaurantImageAsync(this.restaurant$.value.id, 'banner', banner)
      .pipe(take(1))
      .subscribe(() => {
        this.isUpdating$.next(false);
        this.updateError$.next(undefined);

        if (!this.restaurant$.value.imageTypes) {
          this.restaurant$.value.imageTypes = new Array<string>();
        }
        if (!this.restaurant$.value.imageTypes.some(en => en === 'banner')) {
          this.restaurant$.value.imageTypes.push('banner');
        }

        this.restaurant$.next(this.restaurant$.value)
      }, (response: HttpErrorResponse) => {
        this.isUpdating$.next(false);
        this.updateError$.next(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

  removeBanner(): void {
    this.isUpdating$.next(true);
    this.restaurantAdminService.changeRestaurantImageAsync(this.restaurant$.value.id, 'banner', undefined)
      .pipe(take(1))
      .subscribe(() => {
        this.isUpdating$.next(false);
        this.updateError$.next(undefined);

        this.restaurant$.value.imageTypes = this.restaurant$.value.imageTypes.filter(en => en !== 'banner');

        this.restaurant$.next(this.restaurant$.value)
      }, (response: HttpErrorResponse) => {
        this.isUpdating$.next(false);
        this.updateError$.next(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

  changeServiceInfo(serviceInfo: ServiceInfoModel): void {
    this.isUpdating$.next(true);
    this.restaurantAdminService.changeRestaurantServiceInfoAsync(this.restaurant$.value.id, serviceInfo)
      .pipe(take(1))
      .subscribe(() => {
        this.isUpdating$.next(false);
        this.updateError$.next(undefined);

        this.restaurant$.value.pickupInfo = new PickupInfoModel({
          enabled: serviceInfo.pickupEnabled,
          averageTime: serviceInfo.pickupAverageTime,
          minimumOrderValue: serviceInfo.pickupMinimumOrderValue,
          maximumOrderValue: serviceInfo.pickupMaximumOrderValue,
        });

        this.restaurant$.value.deliveryInfo = new DeliveryInfoModel({
          enabled: serviceInfo.deliveryEnabled,
          averageTime: serviceInfo.deliveryAverageTime,
          minimumOrderValue: serviceInfo.deliveryMinimumOrderValue,
          maximumOrderValue: serviceInfo.deliveryMaximumOrderValue,
          costs: serviceInfo.deliveryCosts
        });

        this.restaurant$.value.reservationInfo = new ReservationInfoModel({
          enabled: serviceInfo.reservationEnabled
        });

        this.restaurant$.value.hygienicHandling = serviceInfo.hygienicHandling;

        this.restaurant$.next(this.restaurant$.value)
      }, (response: HttpErrorResponse) => {
        this.isUpdating$.next(false);
        this.updateError$.next(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

}
