import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { RestaurantModel } from '../restaurant/restaurant.model';

@Injectable()
export class RestaurantSysAdminService {
  private baseUrl: string = "api/v1/systemadmin";

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  public searchForRestaurantsAsync(search: string): Observable<RestaurantModel[]> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<RestaurantModel[]>(this.baseUrl + '/restaurants?search=' + encodeURIComponent(search), httpOptions);
  }

  public addRestaurantAsync(name: string): Observable<RestaurantModel> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<RestaurantModel>(this.baseUrl + '/restaurants', { name: name }, httpOptions);
  }

  public removeRestaurantAsync(restaurantId: string): Observable<void> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.delete<void>(this.baseUrl + '/restaurants/' + restaurantId, httpOptions);
  }
}
