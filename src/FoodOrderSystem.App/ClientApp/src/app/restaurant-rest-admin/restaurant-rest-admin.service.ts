import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { RestaurantModel, AddressModel } from '../restaurant/restaurant.model';

@Injectable()
export class RestaurantRestAdminService {
  private baseUrl: string = "api/v1/restaurantadmin";

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

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

  public changeRestaurantNameAsync(id: string, name: string): Observable<RestaurantModel> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<RestaurantModel>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/changename", { name: name }, httpOptions);
  }

  public changeRestaurantAddressAsync(id: string, address: AddressModel): Observable<RestaurantModel> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<RestaurantModel>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/changeaddress", address, httpOptions);
  }

  public changeRestaurantContactDetailsAsync(id: string, webSite: string, imprint: string): Observable<RestaurantModel> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<RestaurantModel>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/changecontactdetails", { webSite: webSite, imprint: imprint }, httpOptions);
  }

  public changeRestaurantDeliveryDataAsync(id: string, minimumOrderValue: number, deliveryCosts: number): Observable<RestaurantModel> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<RestaurantModel>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + "/changedeliverydata", { minimumOrderValue: minimumOrderValue, deliveryCosts: deliveryCosts }, httpOptions);
  }

}
