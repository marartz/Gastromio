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

    //         this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);
  }

  ngOnDestroy() {
  }

  selectTab(tab: string): void {
    this.facade.selectTab(tab);
  }

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

