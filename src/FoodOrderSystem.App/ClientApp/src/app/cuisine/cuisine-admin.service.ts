import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AuthService} from '../auth/auth.service';
import {CuisineModel} from './cuisine.model';

@Injectable()
export class CuisineAdminService {
  private baseUrl = 'api/v1/systemadmin';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
  }

  public getAllCuisinesAsync(): Observable<CuisineModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<CuisineModel[]>(this.baseUrl + '/cuisines', httpOptions);
  }

  public addCuisineAsync(name: string): Observable<CuisineModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<CuisineModel>(this.baseUrl + '/cuisines', {name}, httpOptions);
  }

  public changeCuisineAsync(cuisineId: string, name: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/cuisines/' + cuisineId + '/change', {name}, httpOptions);
  }

  public removeCuisineAsync(cuisineId: string): Observable<void> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.delete<void>(this.baseUrl + '/cuisines/' + cuisineId, httpOptions);
  }
}
