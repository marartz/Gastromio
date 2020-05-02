import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlingService {

  constructor() { }

  handleError(error: HttpErrorResponse): string {
    if (error.status === 401) {
      return "Ihre Anmeldung ist abgelaufen! Bitte melden Sie sich erneut an.";
    } else if (error.status === 403) {
      return "Sie sind nicht berechtigt, diese Aktion auszuführen!";
    } else {
      return error.error;
    }
  }
}
