import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';

import {Observable} from 'rxjs';
import {take} from 'rxjs/operators';

import {AuthService} from '../../auth/services/auth.service';

import {RestaurantModel} from '../../shared/models/restaurant.model';
import {CuisineModel} from '../../shared/models/cuisine.model';

import {CheckoutModel} from '../models/checkout.model';
import {OrderModel} from '../models/order.model';
import {OrderType} from "../models/order-type";

@Injectable()
export class OrderService {

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
  }

  private baseUrl = 'api/v1/order';

  public getAllCuisinesAsync(): Observable<CuisineModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };

    return this.http.get<CuisineModel[]>(this.baseUrl + '/cuisines', httpOptions)
      .pipe(take(1));
  }

  public searchForRestaurantsAsync(search: string, orderType: OrderType, cuisineId: string,
                                   openingHourFilter: string | undefined): Observable<RestaurantModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };

    let orderTypeText: string;
    switch (orderType) {
      case OrderType.Pickup:
        orderTypeText = 'pickup';
        break;
      case OrderType.Delivery:
        orderTypeText = 'delivery';
        break;
      case OrderType.Reservation:
        orderTypeText = 'reservation';
    }

    let url = this.baseUrl + '/restaurants?search=' + encodeURIComponent(search);

    if (orderType != undefined) {
      url += '&orderType=' + encodeURIComponent(orderTypeText);
    }

    if (cuisineId) {
      url += '&cuisineId=' + encodeURIComponent(cuisineId);
    }

    if (openingHourFilter !== undefined) {
      const date = Date.parse(openingHourFilter);
      if (!isNaN(date)) {
        url += '&openingHour=' + encodeURIComponent(openingHourFilter);
      }
    }

    return this.http.get<RestaurantModel[]>(url, httpOptions)
      .pipe(take(1));
  }

  public getRestaurantAsync(id: string): Observable<RestaurantModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<RestaurantModel>(this.baseUrl + '/restaurants/' + encodeURIComponent(id), httpOptions)
      .pipe(take(1));
  }

  public sendCheckoutAsync(checkoutModel: CheckoutModel): Observable<OrderModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json'
      })
    };
    return this.http.post<OrderModel>(this.baseUrl + '/checkout', checkoutModel, httpOptions)
      .pipe(take(1));
  }

}
