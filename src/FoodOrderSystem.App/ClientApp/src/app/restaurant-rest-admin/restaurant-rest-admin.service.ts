import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AuthService} from '../auth/auth.service';
import {RestaurantModel, AddressModel} from '../restaurant/restaurant.model';
import {PaymentMethodModel} from '../payment-method/payment-method.model';
import {UserModel} from '../user/user.model';
import {DishCategoryModel} from '../dish-category/dish-category.model';
import {DishModel} from '../dish-category/dish.model';
import {CuisineModel} from '../cuisine/cuisine.model';

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

  public searchForUsersAsync(search: string): Observable<UserModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<UserModel[]>(this.baseUrl + '/users?search=' + encodeURIComponent(search), httpOptions);
  }

  public changeRestaurantNameAsync(id: string, name: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changename', {name}, httpOptions);
  }

  public changeRestaurantImageAsync(id: string, image: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changeimage', {image}, httpOptions);
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

  public changeRestaurantContactDetailsAsync(id: string, phone: string, webSite: string, imprint: string,
                                             orderEmailAddress: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changecontactdetails', {
      phone,
      webSite,
      imprint,
      orderEmailAddress
    }, httpOptions);
  }

  public changeRestaurantDeliveryDataAsync(id: string, minimumOrderValue: number, deliveryCosts: number): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/changedeliverydata', {
      minimumOrderValue,
      deliveryCosts
    }, httpOptions);
  }

  public addDeliveryTimeToRestaurantAsync(id: string, dayOfWeek: number, start: number, end: number): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/adddeliverytime', {
      dayOfWeek,
      start,
      end
    }, httpOptions);
  }

  public removeDeliveryTimeFromRestaurantAsync(id: string, dayOfWeek: number, start: number): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/removedeliverytime', {
      dayOfWeek,
      start
    }, httpOptions);
  }

  public addCuisineToRestaurantAsync(id: string, cuisineId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/addcuisine', {cuisineId}, httpOptions);
  }

  public removeCuisineFromRestaurantAsync(id: string, cuisineId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/removecuisine', {cuisineId}, httpOptions);
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

  public addAdminToRestaurantAsync(id: string, userId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/addadmin', {userId}, httpOptions);
  }

  public removeAdminFromRestaurantAsync(id: string, userId: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/removeadmin', {userId}, httpOptions);
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
}
