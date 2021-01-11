import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';

import {Observable} from 'rxjs';

import {UserModel} from '../../shared/models/user.model';
import {PagingModel} from '../../shared/components/pagination/paging.model';

import {AuthService} from '../../auth/services/auth.service';
import {take} from "rxjs/operators";

@Injectable()
export class UserAdminService {
  private baseUrl = 'api/v1/systemadmin';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
  }

  public searchForUsersAsync(search: string, skipCount: number, takeCount: number): Observable<PagingModel<UserModel>> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<PagingModel<UserModel>>(this.baseUrl + '/users?search=' + encodeURIComponent(search)
      + '&skip=' + skipCount + '&take=' + takeCount, httpOptions)
      .pipe(take(1));
  }

  public addUserAsync(role: string, email: string, password: string): Observable<UserModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<UserModel>(this.baseUrl + '/users', {role, email, password}, httpOptions)
      .pipe(take(1));
  }

  public changeUserDetailsAsync(userId: string, role: string, email: string): Observable<UserModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<UserModel>(this.baseUrl + '/users/' + userId + '/changedetails', {role, email}, httpOptions)
      .pipe(take(1));
  }

  public changeUserPasswordAsync(userId: string, password: string): Observable<UserModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<UserModel>(this.baseUrl + '/users/' + userId + '/changepassword', {password}, httpOptions)
      .pipe(take(1));
  }

  public removeUserAsync(userId: string): Observable<void> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.delete<void>(this.baseUrl + '/users/' + userId, httpOptions)
      .pipe(take(1));
  }
}
