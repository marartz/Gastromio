import {Injectable} from "@angular/core";
import {HttpErrorResponse} from "@angular/common/http";

import {BehaviorSubject, combineLatest, Observable, throwError} from "rxjs";
import {catchError, map, take, tap} from "rxjs/operators";

import {CuisineModel} from "../shared/models/cuisine.model";
import {DishCategoryModel} from "../shared/models/dish-category.model";
import {DishModel} from "../shared/models/dish.model";
import {
  AddressModel,
  ContactInfoModel,
  DeliveryInfoModel, OpeningPeriodModel,
  PickupInfoModel,
  ReservationInfoModel,
  RestaurantModel,
  ServiceInfoModel
} from "../shared/models/restaurant.model";
import {PaymentMethodModel} from "../shared/models/payment-method.model";

import {HttpErrorHandlingService} from "../shared/services/http-error-handling.service";


@Injectable()
export class SystemAdminFacade {

  private isUpdating$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private updateError$: BehaviorSubject<string> = new BehaviorSubject<string>(undefined);

  static earliestOpeningTime: number = 4 * 60;

  constructor(
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

}
