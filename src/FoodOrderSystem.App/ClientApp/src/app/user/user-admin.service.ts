import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AuthService} from '../auth/auth.service';
import {UserModel} from './user.model';
import {PagingModel} from '../pagination/paging.model';

@Injectable()
export class UserAdminService {
  private baseUrl = 'api/v1/systemadmin';

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
  }

  public searchForUsersAsync(search: string, skip: number, take: number): Observable<PagingModel<UserModel>> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<PagingModel<UserModel>>(this.baseUrl + '/users?search=' + encodeURIComponent(search)
      + '&skip=' + skip + '&take=' + take, httpOptions);
  }

  public addUserAsync(role: string, email: string, password: string): Observable<UserModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<UserModel>(this.baseUrl + '/users', {role, email, password}, httpOptions);
  }

  public changeUserDetailsAsync(userId: string, role: string, email: string): Observable<UserModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<UserModel>(this.baseUrl + '/users/' + userId + '/changedetails', {role, email}, httpOptions);
  }

  public changeUserPasswordAsync(userId: string, password: string): Observable<UserModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<UserModel>(this.baseUrl + '/users/' + userId + '/changepassword', {password}, httpOptions);
  }

  public removeUserAsync(userId: string): Observable<void> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.delete<void>(this.baseUrl + '/users/' + userId, httpOptions);
  }
}