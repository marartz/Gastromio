import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { RestaurantModel, AddressModel } from '../restaurant/restaurant.model';
import { PaymentMethodModel } from '../payment-method/payment-method.model';
import { UserModel } from '../user/user.model';

@Injectable()
export class RestaurantRestAdminService {
  private baseUrl: string = "api/v1/restaurantadmin";

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  public getMyRestaurantsAsync(): Observable<RestaurantModel[]> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<RestaurantModel[]>(this.baseUrl + '/myrestaurants', httpOptions);
  }

  public getRestaurantAsync(id: string): Observable<RestaurantModel> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<RestaurantModel>(this.baseUrl + '/restaurants/' + encodeURIComponent(id), httpOptions);
  }

  public getPaymentMethodsAsync(): Observable<PaymentMethodModel[]> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<PaymentMethodModel[]>(this.baseUrl + '/paymentmethods', httpOptions);
  }

  public searchForUsersAsync(search: string): Observable<UserModel[]> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<UserModel[]>(this.baseUrl + '/users?search=' + encodeURIComponent(search), httpOptions);
  }

  public changeRestaurantNameAsync(id: string, name: string): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/changename", { name: name }, httpOptions);
  }

  public changeRestaurantImageAsync(id: string, image: string): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/changeimage", { image: image}, httpOptions);
  }

  public changeRestaurantAddressAsync(id: string, address: AddressModel): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/changeaddress", address, httpOptions);
  }

  public changeRestaurantContactDetailsAsync(id: string, phone: string, webSite: string, imprint: string, orderEmailAddress: string): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/changecontactdetails", { phone: phone, webSite: webSite, imprint: imprint, orderEmailAddress: orderEmailAddress }, httpOptions);
  }

  public changeRestaurantDeliveryDataAsync(id: string, minimumOrderValue: number, deliveryCosts: number): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/changedeliverydata", { minimumOrderValue: minimumOrderValue, deliveryCosts: deliveryCosts }, httpOptions);
  }

  public addDeliveryTimeToRestaurantAsync(id: string, dayOfWeek: number, start: number, end: number): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/adddeliverytime", { dayOfWeek: dayOfWeek, start: start, end: end }, httpOptions);
  }

  public removeDeliveryTimeFromRestaurantAsync(id: string, dayOfWeek: number, start: number): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/removedeliverytime", { dayOfWeek: dayOfWeek, start: start }, httpOptions);
  }

  public addPaymentMethodToRestaurantAsync(id: string, paymentMethodId: string): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/addpaymentmethod", { paymentMethodId: paymentMethodId }, httpOptions);
  }

  public removePaymentMethodFromRestaurantAsync(id: string, paymentMethodId: string): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/removepaymentmethod", { paymentMethodId: paymentMethodId }, httpOptions);
  }

  public addAdminToRestaurantAsync(id: string, userId: string): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/addadmin", { userId: userId }, httpOptions);
  }

  public removeAdminFromRestaurantAsync(id: string, userId: string): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/removeadmin", { userId: userId }, httpOptions);
  }

}
