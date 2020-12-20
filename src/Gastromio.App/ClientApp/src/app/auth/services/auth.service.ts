import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders, HttpErrorResponse} from '@angular/common/http';

import {Observable, Observer} from 'rxjs';
import {map, take} from 'rxjs/operators';

import {UserModel} from '../../shared/models/user.model';

@Injectable()
export class AuthService {
  private loginUrl = 'api/v1/auth/login';
  private requestPasswordChangeUrl = 'api/v1/auth/requestpasswordchange';
  private validatePasswordResetCodeUrl = 'api/v1/auth/validatepasswordresetcode';
  private changePasswordWithResetCodeUrl = 'api/v1/auth/changepasswordwithresetcode';
  private pingUrl = 'api/v1/auth/ping';

  constructor(
    private http: HttpClient
  ) {
  }

  public isAuthenticated(): boolean {
    return this.getToken() !== undefined && this.getUser() !== undefined;
  }

  public getToken(): string {
    const token = localStorage.getItem('token');
    if (token === null) {
      return undefined;
    }
    return token;
  }

  public getUser(): UserModel {
    const userJSON = localStorage.getItem('user');
    if (userJSON === null) {
      return undefined;
    }
    return JSON.parse(userJSON);
  }

  public loginAsync(email: string, password: string): Observable<{}> {
    return new Observable((observer: Observer<{}>) => {
      const httpOptions = {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          Accept: 'application/json',
        })
      };

      this.http.post<LoginResultModel>(this.loginUrl, {email, password}, httpOptions)
        .pipe(take(1))
        .subscribe(
          (loginResult: LoginResultModel) => {
            localStorage.setItem('token', loginResult.token);
            localStorage.setItem('user', JSON.stringify(loginResult.user));
            observer.next({});
          },
          (err: HttpErrorResponse) => {
            observer.error(err);
          },
          () => {
            observer.complete();
          });

      return () => {
      };
    });
  }

  public requestPasswordChangeAsync(email: string): Observable<void> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
      })
    };

    return this.http.post(this.requestPasswordChangeUrl, {userEmail: email}, httpOptions)
      .pipe(
        take(1),
        map(m => {})
      );
  }

  public validatePasswordResetCodeAsync(userId: string, passwordResetCode: string): Observable<void> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
      })
    };

    return this.http.post(this.validatePasswordResetCodeUrl, {userId, passwordResetCode}, httpOptions)
      .pipe(
        take(1),
        map(m => {})
      );
  }

  public changePasswordWithResetCodeAsync(userId: string, passwordResetCode: string, password: string): Observable<void> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
      })
    };

    return this.http.post(this.changePasswordWithResetCodeUrl, {userId, passwordResetCode, password}, httpOptions)
      .pipe(
        take(1),
        map(m => {})
      );
  }

  public pingAsync(): Observable<{}> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.getToken(),
      })
    };

    return this.http.get<{}>(this.pingUrl, httpOptions);
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
