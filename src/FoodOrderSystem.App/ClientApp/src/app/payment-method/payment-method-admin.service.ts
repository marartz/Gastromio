import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { PaymentMethodModel } from './payment-method.model';

@Injectable()
export class PaymentMethodAdminService {
  private baseUrl: string = "api/v1/systemadmin";

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) { }

  public getAllPaymentMethodsAsync(): Observable<PaymentMethodModel[]> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<PaymentMethodModel[]>(this.baseUrl + '/paymentmethods', httpOptions);
  }

  public addPaymentMethodAsync(name: string, description: string): Observable<PaymentMethodModel> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<PaymentMethodModel>(this.baseUrl + '/paymentmethods', { name: name, description: description }, httpOptions);
  }

  public changePaymentMethodAsync(paymentMethodId: string, name: string, description: string): Observable<boolean> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.post<boolean>(this.baseUrl + '/paymentmethods/' + paymentMethodId + '/change', { name: name, description: description }, httpOptions);
  }

  public removePaymentMethodAsync(paymentMethodId: string): Observable<void> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        'Authorization': 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.delete<void>(this.baseUrl + '/paymentmethods/' + paymentMethodId, httpOptions);
  }
}
