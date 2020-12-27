import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';

import {Observable} from 'rxjs';

import {CuisineModel} from '../../shared/models/cuisine.model';
import {DishCategoryModel} from '../../shared/models/dish-category.model';
import {DishModel} from '../../shared/models/dish.model';
import {RestaurantModel,AddressModel,ContactInfoModel,ServiceInfoModel} from '../../shared/models/restaurant.model';
import {PaymentMethodModel} from '../../shared/models/payment-method.model';

import {AuthService} from '../../auth/services/auth.service';
import {DateModel} from "../../shared/models/date.model";

@Injectable()
export class RestaurantRestAdminService {
  private baseUrl = 'api/v1/restaurantadmin';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
  }

  public getMyRestaurantsAsync(): Observable<RestaurantModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<RestaurantModel[]>(this.baseUrl + '/myrestaurants', httpOptions);
  }

  public getRestaurantAsync(id: string): Observable<RestaurantModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<RestaurantModel>(this.baseUrl + '/restaurants/' + encodeURIComponent(id), httpOptions);
  }

  public getDishesOfRestaurantAsync(id: string): Observable<DishCategoryModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<DishCategoryModel[]>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/dishes', httpOptions);
  }

  public getCuisinesAsync(): Observable<CuisineModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<CuisineModel[]>(this.baseUrl + '/cuisines', httpOptions);
  }

  public getPaymentMethodsAsync(): Observable<PaymentMethodModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<PaymentMethodModel[]>(this.baseUrl + '/paymentmethods', httpOptions);
  }

  public changeRestaurantImageAsync(id: string, type: string, image: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changeimage',
      {type, image}, httpOptions);
  }

  public changeRestaurantAddressAsync(id: string, address: AddressModel): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changeaddress', address, httpOptions);
  }

  public changeRestaurantContactInfoAsync(id: string, contactInfo: ContactInfoModel): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changecontactinfo', {
      phone: contactInfo.phone,
      fax: contactInfo.fax,
      webSite: contactInfo.webSite,
      responsiblePerson: contactInfo.responsiblePerson,
      emailAddress: contactInfo.emailAddress,
      mobile: contactInfo.mobile,
      orderNotificationByMobile: contactInfo.orderNotificationByMobile
    }, httpOptions);
  }

  public addRegularOpeningPeriodToRestaurantAsync(id: string, dayOfWeek: number, start: number, end: number): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/addregularopeningperiod', {
      dayOfWeek,
      start,
      end
    }, httpOptions);
  }

  public changeRegularOpeningPeriodOfRestaurantAsync(id: string, dayOfWeek: number, oldStart: number, newStart: number, newEnd: number): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changeregularopeningperiod', {
      dayOfWeek,
      oldStart,
      newStart,
      newEnd
    }, httpOptions);
  }

  public removeRegularOpeningPeriodFromRestaurantAsync(id: string, dayOfWeek: number, start: number): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/removeregularopeningperiod', {
      dayOfWeek,
      start
    }, httpOptions);
  }

  public addDeviatingOpeningDayToRestaurantAsync(id: string, date: DateModel, status: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/adddeviatingopeningday', {
      date,
      status
    }, httpOptions);
  }

  public changeDeviatingOpeningDayStatusOfRestaurantAsync(id: string, date: DateModel, status: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changedeviatingopeningdaystatus', {
      date,
      status
    }, httpOptions);
  }

  public removeDeviatingOpeningDayFromRestaurantAsync(id: string, date: DateModel): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/removedeviatingopeningday', {
      date
    }, httpOptions);
  }

  public addDeviatingOpeningPeriodToRestaurantAsync(id: string, date: DateModel, start: number, end: number): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/adddeviatingopeningperiod', {
      date,
      start,
      end
    }, httpOptions);
  }

  public changeDeviatingOpeningPeriodOfRestaurantAsync(id: string, date: DateModel, oldStart: number, newStart: number, newEnd: number): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changedeviatingopeningperiod', {
      date,
      oldStart,
      newStart,
      newEnd
    }, httpOptions);
  }

  public removeDeviatingOpeningPeriodFromRestaurantAsync(id: string, date: DateModel, start: number): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/removedeviatingopeningperiod', {
      date,
      start
    }, httpOptions);
  }

  public changeRestaurantServiceInfoAsync(id: string, serviceInfo: ServiceInfoModel): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changeserviceinfo', {
      pickupEnabled: serviceInfo.pickupEnabled,
      pickupAverageTime: serviceInfo.pickupAverageTime,
      pickupMinimumOrderValue: serviceInfo.pickupMinimumOrderValue,
      pickupMaximumOrderValue: serviceInfo.pickupMaximumOrderValue,
      deliveryEnabled: serviceInfo.deliveryEnabled,
      deliveryAverageTime: serviceInfo.deliveryAverageTime,
      deliveryMinimumOrderValue: serviceInfo.deliveryMinimumOrderValue,
      deliveryMaximumOrderValue: serviceInfo.deliveryMaximumOrderValue,
      deliveryCosts: serviceInfo.deliveryCosts,
      reservationEnabled: serviceInfo.reservationEnabled,
      hygienicHandling: serviceInfo.hygienicHandling
    }, httpOptions);
  }

  public addPaymentMethodToRestaurantAsync(id: string, paymentMethodId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/addpaymentmethod',
      {paymentMethodId}, httpOptions);
  }

  public removePaymentMethodFromRestaurantAsync(id: string, paymentMethodId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/removepaymentmethod',
      {paymentMethodId}, httpOptions);
  }

  public addDishCategoryToRestaurantAsync(id: string, name: string, afterCategoryId: string): Observable<string> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<string>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/adddishcategory',
      {name, afterCategoryId}, httpOptions);
  }

  public changeDishCategoryOfRestaurantAsync(id: string, dishCategoryId: string, name: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changedishcategory', {
      dishCategoryId,
      name
    }, httpOptions);
  }

  public incOrderOfDishCategoryAsync(id: string, dishCategoryId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/incorderofdishcategory',
      {dishCategoryId}, httpOptions);
  }

  public decOrderOfDishCategoryAsync(id: string, dishCategoryId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/decorderofdishcategory',
      {dishCategoryId}, httpOptions);
  }

  public removeDishCategoryFromRestaurantAsync(id: string, dishCategoryId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/removedishcategory',
      {dishCategoryId}, httpOptions);
  }

  public addOrChangeDishOfRestaurantAsync(id: string, dishCategoryId: string, dish: DishModel): Observable<string> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<string>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/addoreditdish', {
      dishCategoryId,
      dish
    }, httpOptions);
  }

  public incOrderOfDishAsync(id: string, dishId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/incorderofdish',
      {dishId}, httpOptions);
  }

  public decOrderOfDishAsync(id: string, dishId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/decorderofdish',
      {dishId}, httpOptions);
  }

  public removeDishFromRestaurantAsync(id: string, dishCategoryId: string, dishId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/removedish', {
      dishCategoryId,
      dishId
    }, httpOptions);
  }

  public changeSupportedOrderMode(id: string, supportedOrderMode: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changesupportedordermode', {
      supportedOrderMode,
    }, httpOptions);
  }

  public addOrChangeExternalMenu(id: string, externalMenuId: string, name: string, description: string, url: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/addorchangeexternalmenu', {
      externalMenuId,
      name,
      description,
      url
    }, httpOptions);
  }

  public removeExternalMenu(id: string, externalMenuId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/removeexternalmenu', {
      externalMenuId,
    }, httpOptions);
  }

}
