import {Injectable} from "@angular/core";
import {HttpErrorResponse} from "@angular/common/http";

import {BehaviorSubject, combineLatest, Observable} from "rxjs";
import {map, take, tap} from "rxjs/operators";

import {
  AddressModel,
  ContactInfoModel, DeliveryInfoModel,
  PickupInfoModel, ReservationInfoModel,
  RestaurantModel,
  ServiceInfoModel
} from "../shared/models/restaurant.model";

import {RestaurantRestAdminService} from "./services/restaurant-rest-admin.service";
import {HttpErrorHandlingService} from "../shared/services/http-error-handling.service";
import {CuisineModel} from "../shared/models/cuisine.model";
import {PaymentMethodModel} from "../shared/models/payment-method.model";
import {DishCategoryModel} from "../shared/models/dish-category.model";

@Injectable()
export class RestaurantAdminFacade {

  private restaurantId: string;

  private isInitializing$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  private isInitialized$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(undefined);
  private initializationError$: BehaviorSubject<string> = new BehaviorSubject<string>(undefined);

  private cuisines$: BehaviorSubject<CuisineModel[]> = new BehaviorSubject<CuisineModel[]>(undefined);
  private paymentMethods$: BehaviorSubject<PaymentMethodModel[]> = new BehaviorSubject<PaymentMethodModel[]>(undefined);
  private restaurant$: BehaviorSubject<RestaurantModel> = new BehaviorSubject<RestaurantModel>(undefined);
  private dishCategories$: BehaviorSubject<DishCategoryModel[]> = new BehaviorSubject<DishCategoryModel[]>(undefined);

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

    const getCuisines$ = this.restaurantAdminService.getCuisinesAsync()
      .pipe(
        tap(cuisines => {
          this.cuisines$.next(cuisines);
        })
      );

    const getPaymentMethods$ = this.restaurantAdminService.getPaymentMethodsAsync()
      .pipe(
        tap(paymentMethods => {
          this.paymentMethods$.next(paymentMethods.sort((a, b) => {
            if (a.name < b.name) {
              return -1;
            }
            if (a.name > b.name) {
              return 1;
            }
            return 0;
          }));
        })
      );

    const getRestaurant$ = this.restaurantAdminService.getRestaurantAsync(restaurantId)
      .pipe(
        tap(restaurant => {
          restaurant.cuisines.sort((a, b) => {
            if (a.name < b.name) {
              return -1;
            }
            if (a.name > b.name) {
              return 1;
            }
            return 0;
          });

          restaurant.paymentMethods.sort((a, b) => {
            if (a.name < b.name) {
              return -1;
            }
            if (a.name > b.name) {
              return 1;
            }
            return 0;
          });

          this.restaurant$.next(restaurant);
        })
      );

    const getDishesOfRestaurant$ = this.restaurantAdminService.getDishesOfRestaurantAsync(restaurantId)
      .pipe(
        tap(dishCategories => {
          this.dishCategories$.next(dishCategories);
        })
      )

    const observables = [
      getCuisines$,
      getPaymentMethods$,
      getRestaurant$,
      getDishesOfRestaurant$
    ];

    combineLatest(observables)
      .subscribe(
        () => {

          //         this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);

          //               if (this.dishCategories !== undefined && this.dishCategories.length > 0) {
          //                 this.activeDishCategoryId = this.dishCategories[0].id;

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

  public getCuisines$(): Observable<CuisineModel[]> {
    return this.cuisines$;
  }

  public getPaymentMethods$(): Observable<PaymentMethodModel[]> {
    return this.paymentMethods$;
  }

  public getRestaurant$(): Observable<RestaurantModel> {
    return this.restaurant$;
  }

  public getDishCategories$(): Observable<DishCategoryModel[]> {
    return this.dishCategories$;
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

  public getCuisineStatus$(): Observable<{}> {
    return this.restaurant$.asObservable()
      .pipe(
        map(restaurant => {
          const result = {};
          for (let cuisine of this.cuisines$.value) {
            result[cuisine.id] = restaurant.cuisines.some(en => en.id === cuisine.id);
          }
          return result;
        })
      );
  }

  public getPaymentMethodStatus$(): Observable<{}> {
    return this.restaurant$.asObservable()
      .pipe(
        map(restaurant => {
          const result = {};
          for (let paymentMethod of this.paymentMethods$.value) {
            result[paymentMethod.id] = restaurant.paymentMethods.some(en => en.id === paymentMethod.id);
          }
          return result;
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

  toggleCuisine(cuisineId: string): void {
    this.isUpdating$.next(true);

    let observable: Observable<boolean>;

    const isCurrentlyEnabled = this.restaurant$.value.cuisines.some(en => en.id === cuisineId);
    if (isCurrentlyEnabled) {
      observable = this.restaurantAdminService.removeCuisineFromRestaurantAsync(this.restaurant$.value.id, cuisineId);
    } else {
      observable = this.restaurantAdminService.addCuisineToRestaurantAsync(this.restaurant$.value.id, cuisineId);
    }

    observable
      .pipe(take(1))
      .subscribe(() => {
        this.isUpdating$.next(false);
        this.updateError$.next(undefined);

        if (isCurrentlyEnabled) {
          this.restaurant$.value.cuisines = this.restaurant$.value.cuisines.filter(en => en.id !== cuisineId);
        } else {
          const cuisine = this.cuisines$.value.find(en => en.id === cuisineId);
          this.restaurant$.value.cuisines.push(cuisine);
          this.restaurant$.value.cuisines.sort((a, b) => {
            if (a.name < b.name) {
              return -1;
            }
            if (a.name > b.name) {
              return 1;
            }
            return 0;
          });
        }

        this.restaurant$.next(this.restaurant$.value)
      }, (response: HttpErrorResponse) => {
        this.isUpdating$.next(false);
        this.updateError$.next(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

  togglePaymentMethod(paymentMethodId: string): void {
    this.isUpdating$.next(true);

    let observable: Observable<boolean>;

    const isCurrentlyEnabled = this.restaurant$.value.paymentMethods.some(en => en.id === paymentMethodId);
    if (isCurrentlyEnabled) {
      observable = this.restaurantAdminService.removePaymentMethodFromRestaurantAsync(this.restaurant$.value.id, paymentMethodId);
    } else {
      observable = this.restaurantAdminService.addPaymentMethodToRestaurantAsync(this.restaurant$.value.id, paymentMethodId);
    }

    observable
      .pipe(take(1))
      .subscribe(() => {
        this.isUpdating$.next(false);
        this.updateError$.next(undefined);

        if (isCurrentlyEnabled) {
          this.restaurant$.value.paymentMethods = this.restaurant$.value.paymentMethods.filter(en => en.id !== paymentMethodId);
        } else {
          const paymentMethod = this.paymentMethods$.value.find(en => en.id === paymentMethodId);
          this.restaurant$.value.paymentMethods.push(paymentMethod);
          this.restaurant$.value.paymentMethods.sort((a, b) => {
            if (a.name < b.name) {
              return -1;
            }
            if (a.name > b.name) {
              return 1;
            }
            return 0;
          });
        }

        this.restaurant$.next(this.restaurant$.value)
      }, (response: HttpErrorResponse) => {
        this.isUpdating$.next(false);
        this.updateError$.next(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

}
