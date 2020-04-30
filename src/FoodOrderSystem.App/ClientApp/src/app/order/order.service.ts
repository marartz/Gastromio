import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { RestaurantModel } from '../restaurant/restaurant.model';

@Injectable()
export class OrderService {
  private baseUrl: string = "api/v1/order";

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
}
