import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';

import {Observable} from 'rxjs';

import {RestaurantModel} from '../../shared/models/restaurant.model';
import {PagingModel} from '../../shared/components/pagination/paging.model';

import {AuthService} from '../../auth/services/auth.service';

import {ImportLogModel} from '../models/import-log.model';

@Injectable()
export class RestaurantSysAdminService {
  private baseUrl = 'api/v1/systemadmin';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
  }

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
    return this.http.post<RestaurantModel>(this.baseUrl + '/restaurants', {name}, httpOptions);
  }

  public activateRestaurantAsync(id: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/activate',
      {}, httpOptions);
  }

  public deactivateRestaurantAsync(id: string): Observable<boolean> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/deactivate',
      {}, httpOptions);
  }

  public enableSupportForRestaurantAsync(restaurantId: string): Observable<void> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    const url = this.baseUrl + '/restaurants/' + encodeURIComponent(restaurantId) + '/enablesupport';
    return this.http.post<void>(url, {}, httpOptions);
  }

  public disableSupportForRestaurantAsync(restaurantId: string): Observable<void> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    const url = this.baseUrl + '/restaurants/' + encodeURIComponent(restaurantId) + '/disablesupport';
    return this.http.post<void>(url, {}, httpOptions);
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

  public importRestaurantsAsync(importFile: File, dryRun: boolean): Observable<ImportLogModel> {
    const formData: FormData = new FormData();
    formData.append('fileKey', importFile, importFile.name);
    const httpOptions = {
      headers: new HttpHeaders({
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    let url = this.baseUrl + '/restaurants/import';
    if (dryRun) {
      url = url + '?dryrun=true';
    }
    return this.http.post<ImportLogModel>(url, formData, httpOptions);
  }

  public importDishesAsync(importFile: File, dryRun: boolean): Observable<ImportLogModel> {
    const formData: FormData = new FormData();
    formData.append('fileKey', importFile, importFile.name);
    const httpOptions = {
      headers: new HttpHeaders({
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    let url = this.baseUrl + '/dishes/import';
    if (dryRun) {
      url = url + '?dryrun=true';
    }
    return this.http.post<ImportLogModel>(url, formData, httpOptions);
  }
}