import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { RestaurantModel } from '../restaurant/restaurant.model';
import { PagingModel } from '../pagination/paging.model';

@Injectable()
export class RestaurantSysAdminService {
  private baseUrl = 'api/v1/systemadmin';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  public searchForRestaurantsAsync(search: string, skip: number, take: number): Observable<PagingModel<RestaurantModel>> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<PagingModel<RestaurantModel>>(this.baseUrl + '/restaurants?search=' + encodeURIComponent(search)
      + '&skip=' + skip + '&take=' + take, httpOptions);
  }

  public addRestaurantAsync(name: string): Observable<RestaurantModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<RestaurantModel>(this.baseUrl + '/restaurants', { name }, httpOptions);
  }

  public removeRestaurantAsync(restaurantId: string): Observable<void> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.delete<void>(this.baseUrl + '/restaurants/' + restaurantId, httpOptions);
  }
}
