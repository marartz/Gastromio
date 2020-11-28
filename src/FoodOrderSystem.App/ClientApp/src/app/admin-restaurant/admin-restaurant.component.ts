import {Component, OnInit, OnDestroy, ViewChild, ElementRef} from '@angular/core';
import {ActivatedRoute} from '@angular/router';
import {OpeningPeriodModel, RestaurantModel} from '../restaurant/restaurant.model';
import {RestaurantRestAdminService} from '../restaurant-rest-admin/restaurant-rest-admin.service';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ChangeRestaurantNameComponent} from '../change-restaurant-name/change-restaurant-name.component';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
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
import {AuthService} from '../auth/auth.service';
import {Router} from '@angular/router';

@Component({
  selector: 'app-admin-restaurant',
  templateUrl: './admin-restaurant.component.html',
  styleUrls: [
    './admin-restaurant.component.css',
    '../../assets/css/frontend_v2.min.css',
    '../../assets/css/backend_v2.min.css'
  ]
})
export class AdminRestaurantComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  restaurantId: string;
  restaurant: RestaurantModel;

  generalError: string;
  formError: string;

  changeLogoForm: FormGroup;
  logoUrl: string;
  @ViewChild('logo') logoElement: ElementRef;

  changeBannerForm: FormGroup;
  bannerUrl: string;
  @ViewChild('banner') bannerElement: ElementRef;

  changeAddressForm: FormGroup;

  changeContactInfoForm: FormGroup;

  openingPeriodVMs: OpeningPeriodViewModel[];
  addOpeningPeriodForm: FormGroup;

  changeServiceInfoForm: FormGroup;

  availableCuisines: CuisineModel[];
  addCuisineForm: FormGroup;

  paymentMethods: PaymentMethodModel[];
  paymentMethodStatus: boolean[];

  public userToBeAdded: UserModel;

  dishCategories: DishCategoryModel[];
  activeDishCategoryId: string;

  constructor(
    private route: ActivatedRoute,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private restaurantRestAdminService: RestaurantRestAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService,
    private authService: AuthService,
    private router: Router
  ) {
    this.restaurant = new RestaurantModel();

    this.changeLogoForm = this.formBuilder.group({
      logo: ''
    });

    this.changeBannerForm = this.formBuilder.group({
      banner: ''
    });

    this.changeAddressForm = this.formBuilder.group({
      street: ['', [Validators.required, Validators.pattern(/^(([a-zA-ZäöüÄÖÜß]\D*)\s+\d+?\s*.*)$/)]],
      zipCode: ['', [Validators.required, Validators.pattern(/^([0]{1}[1-9]{1}|[1-9]{1}[0-9]{1})[0-9]{3}$/)]],
      city: ['', Validators.required]
    });

    this.changeContactInfoForm = this.formBuilder.group({
      phone: ['', [Validators.required, Validators.pattern(/^(((((((00|\+)49[ \-/]?)|0)[1-9][0-9]{1,4})[ \-/]?)|((((00|\+)49\()|\(0)[1-9][0-9]{1,4}\)[ \-/]?))[0-9]{1,7}([ \-/]?[0-9]{1,5})?)$/)]],
      fax: ['', Validators.pattern(/^(((((((00|\+)49[ \-/]?)|0)[1-9][0-9]{1,4})[ \-/]?)|((((00|\+)49\()|\(0)[1-9][0-9]{1,4}\)[ \-/]?))[0-9]{1,7}([ \-/]?[0-9]{1,5})?)$/)],
      webSite: ['', Validators.pattern(/^(https?:\/\/){0,1}(www\.)?[-a-zäöüA-ZÄÖÜ0-9@:%._\+~#=]{1,256}\.[a-zäöüA-ZÄÖÜ0-9()]{1,6}\b([-a-zäöüA-ZÄÖÜ0-9()@:%_\+.~#?&//=]*)$/)],
      responsiblePerson: ['', Validators.required],
      emailAddress: ['', [Validators.required, Validators.pattern("^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$")]]
    });

    this.addOpeningPeriodForm = this.formBuilder.group({
      dayOfWeek: '',
      start: '',
      end: ''
    });

    this.changeServiceInfoForm = this.formBuilder.group({
      pickupEnabled: false,
      pickupAverageTime: '',
      pickupMinimumOrderValue: '',
      pickupMaximumOrderValue: '',
      deliveryEnabled: false,
      deliveryAverageTime: '',
      deliveryMinimumOrderValue: '',
      deliveryMaximumOrderValue: '',
      deliveryCosts: '',
      reservationEnabled: false,
      hygienicHandling: ''
    });

    this.addCuisineForm = this.formBuilder.group({
      cuisineId: ''
    });
  }

  get af() {
    return this.changeAddressForm.controls;
  }

  get cif() {
    return this.changeContactInfoForm.controls;
  }

  private static parseTimeValue(text: string): TimeParseResult {
    if (!text) {
      return new TimeParseResult(false, 0);
    }

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

      this.restaurantRestAdminService.getRestaurantAsync(this.restaurantId)
        .pipe(take(1))
        .subscribe(
          (data) => {
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

            if (this.hasLogo()) {
              this.updateLogoUrl();
            }

            if (this.hasBanner()) {
              this.updateBannerUrl();
            }

            this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);

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

            this.changeServiceInfoForm.patchValue({
              pickupEnabled: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.enabled : false,
              pickupAverageTime: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.averageTime : '',
              pickupMinimumOrderValue: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.minimumOrderValue : '',
              pickupMaximumOrderValue: this.restaurant.pickupInfo != null ? this.restaurant.pickupInfo.maximumOrderValue : '',
              deliveryEnabled: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.enabled : false,
              deliveryAverageTime: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.averageTime : '',
              deliveryMinimumOrderValue: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.minimumOrderValue : '',
              deliveryMaximumOrderValue: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.maximumOrderValue : '',
              deliveryCosts: this.restaurant.deliveryInfo != null ? this.restaurant.deliveryInfo.costs : '',
              reservationEnabled: this.restaurant.reservationInfo != null ? this.restaurant.reservationInfo.enabled : false,
              hygienicHandling: this.restaurant.hygienicHandling
            });
            this.changeServiceInfoForm.markAsPristine();

            this.restaurantRestAdminService.getDishesOfRestaurantAsync(this.restaurantId)
              .pipe(take(1))
              .subscribe(
                (dishCategories) => {

                  this.dishCategories = dishCategories;

                  if (this.dishCategories !== undefined && this.dishCategories.length > 0) {
                    this.activeDishCategoryId = this.dishCategories[0].id;
                  }

                  this.restaurantRestAdminService.getCuisinesAsync()
                    .pipe(take(1))
                    .subscribe(
                      (cuisines) => {
                        this.availableCuisines = cuisines.filter(cuisine => this.restaurant.cuisines
                          .findIndex(en => en.id === cuisine.id) === -1);

                        this.restaurantRestAdminService.getPaymentMethodsAsync()
                          .pipe(take(1))
                          .subscribe(
                            (paymentMethods) => {
                              this.paymentMethods = paymentMethods.sort((a, b) => {
                                if (a.name < b.name) {
                                  return -1;
                                }
                                if (a.name > b.name) {
                                  return 1;
                                }
                                return 0;
                              });
                              this.paymentMethodStatus = new Array<boolean>(this.paymentMethods.length);

                              for (let i = 0; i < this.paymentMethods.length; i++) {
                                this.paymentMethodStatus[i] = this.restaurant.paymentMethods
                                  .some(en => en.id === this.paymentMethods[i].id);
                              }

                              this.blockUI.stop();
                            },
                            (error: HttpErrorResponse) => {
                              this.blockUI.stop();
                              this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
                            }
                          );
                      },
                      (error: HttpErrorResponse) => {
                        this.blockUI.stop();
                        this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
                      }
                    );

                },
                (error: HttpErrorResponse) => {
                  this.blockUI.stop();
                  this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
                }
              );

          },
          (error: HttpErrorResponse) => {
            this.blockUI.stop();
            this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
          }
        );
    });
  }

  ngOnDestroy() {
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['']);
  }

  getRestaurantStatusText(): string {
    if (!this.restaurant) {
      return '';
    }
    return this.restaurant.isActive ? 'Aktiv' : 'Nicht aktiv';
  }

  onActivate(): void {
    this.blockUI.start('Aktiviere Restaurant...');
    this.restaurantRestAdminService.activateRestaurantAsync(this.restaurant.id)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.formError = undefined;
        this.restaurant.isActive = true;
      }, response => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onDeactivate(): void {
    this.blockUI.start('Deaktiviere Restaurant...');
    this.restaurantRestAdminService.deactivateRestaurantAsync(this.restaurant.id)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.formError = undefined;
        this.restaurant.isActive = false;
      }, response => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  hasLogo(): boolean {
    if (!this.restaurant || !this.restaurant.imageTypes) {
      return false;
    }
    return this.restaurant.imageTypes.some(en => en === 'logo');
  }

  updateLogoUrl(): void {
    if (this.hasLogo()) {
      this.logoUrl = '/api/v1/restaurants/' + this.restaurant?.id + '/images/logo?random=' + Math.random();
    } else {
      this.logoUrl = undefined;
    }
  }

  onLogoChange(event) {
    if (!event.target.files || !event.target.files.length) {
      return;
    }

    const reader = new FileReader();
    const [file] = event.target.files;

    reader.onload = () => {
      this.changeLogoForm.patchValue({
        logo: reader.result
      });
      this.logoUrl = reader.result as string;
      this.changeLogoForm.markAsDirty();
    };

    reader.readAsDataURL(file);
  }

  hasBanner(): boolean {
    if (!this.restaurant || !this.restaurant.imageTypes) {
      return false;
    }
    return this.restaurant.imageTypes.some(en => en === 'banner');
  }

  updateBannerUrl(): void {
    if (this.hasBanner()) {
      this.bannerUrl = '/api/v1/restaurants/' + this.restaurant?.id + '/images/banner?random=' + Math.random();
    } else {
      this.bannerUrl = undefined;
    }
  }

  onBannerChange(event) {
    if (!event.target.files || !event.target.files.length) {
      return;
    }

    const reader = new FileReader();
    const [file] = event.target.files;

    reader.onload = () => {
      this.changeBannerForm.patchValue({
        banner: reader.result
      });
      this.bannerUrl = reader.result as string;
      this.changeBannerForm.markAsDirty();
    };

    reader.readAsDataURL(file);
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

  onSaveLogo(value): void {
    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.changeRestaurantImageAsync(this.restaurant.id, 'logo', value.logo)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.logoElement.nativeElement.value = null;
        this.formError = undefined;
        if (!this.restaurant.imageTypes) {
          this.restaurant.imageTypes = new Array<string>();
        }
        if (!this.restaurant.imageTypes.some(en => en === 'logo')) {
          this.restaurant.imageTypes.push('logo');
        }
        this.updateLogoUrl();
        this.changeLogoForm.markAsPristine();
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onRemoveLogo(): void {
    if (!confirm('Soll das Logo wirklich entfernt werden?')) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.changeRestaurantImageAsync(this.restaurant.id, 'logo', undefined)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.logoElement.nativeElement.value = null;
        this.restaurant.imageTypes = this.restaurant.imageTypes.filter(en => en !== 'logo');
        this.updateLogoUrl();
        this.formError = undefined;
        this.changeLogoForm.markAsPristine();
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onSaveBanner(value): void {
    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.changeRestaurantImageAsync(this.restaurant.id, 'banner', value.banner)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.bannerElement.nativeElement.value = null;
        this.formError = undefined;
        if (!this.restaurant.imageTypes) {
          this.restaurant.imageTypes = new Array<string>();
        }
        if (!this.restaurant.imageTypes.some(en => en === 'banner')) {
          this.restaurant.imageTypes.push('banner');
        }
        this.updateBannerUrl();
        this.changeBannerForm.markAsPristine();
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onRemoveBanner(): void {
    if (!confirm('Soll das Banner wirklich entfernt werden?')) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.changeRestaurantImageAsync(this.restaurant.id, 'banner', undefined)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.bannerElement.nativeElement.value = null;
        this.restaurant.imageTypes = this.restaurant.imageTypes.filter(en => en !== 'banner');
        this.updateBannerUrl();
        this.formError = undefined;
        this.changeBannerForm.markAsPristine();
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onSaveAddress(value): void {
    if (this.changeAddressForm.invalid) {
      return;
    }
    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.changeRestaurantAddressAsync(this.restaurant.id, value)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.formError = undefined;
        this.restaurant.address = value;
        this.changeAddressForm.markAsPristine();
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onSaveContactInfo(value): void {
    if (this.changeContactInfoForm.invalid) {
      return;
    }
    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.changeRestaurantContactInfoAsync(this.restaurant.id, value)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.formError = undefined;
        this.restaurant.contactInfo = value;
        this.changeContactInfoForm.markAsPristine();
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onAddOpeningPeriod(value): void {
    const dayOfWeek: number = Number(value.dayOfWeek);
    const invalidNumberError: string = 'Geben Sie eine gültige Zeit ein';

    const startParseResult: TimeParseResult = AdminRestaurantComponent.parseTimeValue(value.start);
    if (!startParseResult.isValid) {
      this.formError = invalidNumberError;
      return;
    } else {
      this.formError = undefined;
    }

    const endParseResult: TimeParseResult = AdminRestaurantComponent.parseTimeValue(value.end);
    if (!endParseResult.isValid) {
      this.formError = invalidNumberError;
      return;
    } else {
      this.formError = undefined;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.addOpeningPeriodToRestaurantAsync(this.restaurant.id, dayOfWeek,
      startParseResult.value, endParseResult.value)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();

        this.formError = undefined;
        this.addOpeningPeriodForm.reset();

        const model = new OpeningPeriodModel();
        model.dayOfWeek = dayOfWeek;
        model.start = startParseResult.value;
        model.end = endParseResult.value;

        this.restaurant.openingHours.push(model);
        this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onRemoveOpeningPeriod(openingPeriod: OpeningPeriodViewModel) {
    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.removeOpeningPeriodFromRestaurantAsync(this.restaurant.id, openingPeriod.dayOfWeek,
      openingPeriod.startTime)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.formError = undefined;

        const index = this.restaurant.openingHours.findIndex(elem => elem.dayOfWeek === openingPeriod.dayOfWeek
          && elem.start === openingPeriod.startTime);
        if (index > -1) {
          this.restaurant.openingHours.splice(index, 1);
          this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);
        }
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onSaveServiceInfo(value): void {
    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.changeRestaurantServiceInfoAsync(this.restaurant.id, value)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.formError = undefined;
        this.restaurant.pickupInfo = value;
        this.changeServiceInfoForm.markAsPristine();
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onAddCuisine(value): void {
    if (!value || !value.cuisineId)
      return;

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.addCuisineToRestaurantAsync(this.restaurant.id, value.cuisineId)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.formError = undefined;
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
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  onRemoveCuisine(cuisineId: string) {
    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.removeCuisineFromRestaurantAsync(this.restaurant.id, cuisineId)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.formError = undefined;
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
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  isPaymentMethodEnabled(paymentMethod: PaymentMethodModel): boolean {
    const index = this.paymentMethods.findIndex(en => en.id === paymentMethod.id);
    if (index < 0) {
      return false;
    }
    return this.paymentMethodStatus[index];
  }

  onPaymentMethodStatusToggled(paymentMethod: PaymentMethodModel): void {
    const index = this.paymentMethods.findIndex(en => en.id === paymentMethod.id);
    if (index < 0) {
      return;
    }

    if (this.paymentMethods[index].id.toLocaleUpperCase() === '8DBBC822-E4FF-47B6-8CA2-68F4F0C51AA3') {
      return;
    }

    const currentStatus = this.paymentMethodStatus[index];
    this.blockUI.start('Verarbeite Daten...');
    let observable: Observable<boolean>;
    if (currentStatus) {
      observable = this.restaurantRestAdminService.removePaymentMethodFromRestaurantAsync(this.restaurant.id,
        paymentMethod.id);
    } else {
      observable = this.restaurantRestAdminService.addPaymentMethodToRestaurantAsync(this.restaurant.id,
        paymentMethod.id);
    }
    observable
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.generalError = undefined;
        this.paymentMethodStatus[index] = !this.paymentMethodStatus[index];
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
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
    this.restaurantRestAdminService.addAdminToRestaurantAsync(this.restaurant.id, this.userToBeAdded.id)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.formError = undefined;

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
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }

  removeUser(user: UserModel): void {
    this.blockUI.start('Verarbeite Daten...');
    this.restaurantRestAdminService.removeAdminFromRestaurantAsync(this.restaurant.id, user.id)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.formError = undefined;
        const index = this.restaurant.administrators.findIndex(en => en.id === user.id);
        this.restaurant.administrators.splice(index, 1);
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
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
    this.restaurantRestAdminService.incOrderOfDishCategoryAsync(this.restaurant.id, dishCategory.id)
      .pipe(take(1))
      .subscribe(() => {
        [this.dishCategories[pos], this.dishCategories[pos + 1]] = [this.dishCategories[pos + 1], this.dishCategories[pos]];
        this.blockUI.stop();
      }, (response: HttpErrorResponse) => {
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
    this.restaurantRestAdminService.decOrderOfDishCategoryAsync(this.restaurant.id, dishCategory.id)
      .pipe(take(1))
      .subscribe(() => {
        [this.dishCategories[pos - 1], this.dishCategories[pos]] = [this.dishCategories[pos], this.dishCategories[pos - 1]];
        this.blockUI.stop();
      }, (response: HttpErrorResponse) => {
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

  getPricesOfDish(dish: DishModel): string {
    if (!dish.variants || dish.variants.length === 0) {
      return '';
    }

    if (dish.variants.length === 1) {
      return '€' + dish.variants[0].price.toLocaleString('de', {minimumFractionDigits: 2});
    }

    let result = '';
    for (const variant of dish.variants) {
      if (result.length > 0) {
        result += '; ';
      }
      result += variant.name + ' €' + variant.price.toLocaleString('de', {minimumFractionDigits: 2});
    }

    return result;
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
    this.restaurantRestAdminService.incOrderOfDishAsync(this.restaurant.id, dish.id)
      .pipe(take(1))
      .subscribe(() => {
        [dishCategory.dishes[pos], dishCategory.dishes[pos + 1]] = [dishCategory.dishes[pos + 1], dishCategory.dishes[pos]];
        this.blockUI.stop();
      }, (response: HttpErrorResponse) => {
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
    this.restaurantRestAdminService.decOrderOfDishAsync(this.restaurant.id, dish.id)
      .pipe(take(1))
      .subscribe(() => {
        [dishCategory.dishes[pos - 1], dishCategory.dishes[pos]] = [dishCategory.dishes[pos], dishCategory.dishes[pos - 1]];
        this.blockUI.stop();
      }, (response: HttpErrorResponse) => {
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

  public static daysOfMonth = [
    'Montag',
    'Dienstag',
    'Mittwoch',
    'Donnerstag',
    'Freitag',
    'Samstag',
    'Sonntag'
  ];

  dayOfWeek: number;
  dayOfWeekText: string;
  startTime: number;
  startTimeText: string;
  endTime: number;
  endTimeText: string;

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
