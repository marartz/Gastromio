import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, Observer } from 'rxjs';
import { UserModel } from '../user/user.model';

@Injectable()
export class AuthService {
  private loginUrl: string = "api/v1/auth/login";

  constructor(
    private http: HttpClient
  ) { }

  public isAuthenticated(): boolean {
    return this.getToken() !== undefined && this.getUser() !== undefined;
  }

  public getToken(): string {
    let token = localStorage.getItem('token');
    if (token === null)
      return undefined;
    return token;
  }

  public getUser(): UserModel {
    let userJSON = localStorage.getItem('user');
    if (userJSON === null)
      return undefined;
    return JSON.parse(userJSON);
  }

  public loginAsync(username: string, password: string): Observable<{}> {
    return new Observable((observer: Observer<{}>) => {
      let httpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Accept': 'application/json',
        })
      };

      let subscription = this.http.post<LoginResultModel>(this.loginUrl, { username: username, password: password }, httpOptions).subscribe(
        (loginResult: LoginResultModel) => {
          localStorage.setItem('token', loginResult.token);
          localStorage.setItem('user', JSON.stringify(loginResult.user));
          observer.next({});
        },
        (err: HttpErrorResponse) => {
          observer.error(err.status);
        },
        () => {
          observer.complete();
        });

      return () => {
        subscription.unsubscribe();
      };
    });
  }

  public logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  }
}

class LoginResultModel {
  public token: string;
  public user: UserModel;
}
