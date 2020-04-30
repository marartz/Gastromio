import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { UserModel } from './user.model';

@Injectable()
export class UserAdminService {
  private baseUrl: string = "api/v1/systemadmin";

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

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

  public addUserAsync(name: string, role: string, email: string, password: string): Observable<UserModel> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<UserModel>(this.baseUrl + '/users', { name: name, role: role, email: email, password: password }, httpOptions);
  }

  public changeUserDetailsAsync(userId: string, name: string, role: string, email: string): Observable<UserModel> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<UserModel>(this.baseUrl + '/users/' + userId + '/changedetails', { name: name, role: role, email: email }, httpOptions);
  }

  public changeUserPasswordAsync(userId: string, password: string): Observable<UserModel> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<UserModel>(this.baseUrl + '/users/' + userId + '/changepassword', { password: password }, httpOptions);
  }

  public removeUserAsync(userId: string): Observable<void> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.delete<void>(this.baseUrl + '/users/' + userId, httpOptions);
  }
}
