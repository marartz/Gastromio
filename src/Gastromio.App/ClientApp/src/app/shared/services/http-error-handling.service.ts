import {Injectable} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';
import {FailureResult} from '../models/failure-result.model';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlingService {

  constructor() {
  }

  handleError(httpError: HttpErrorResponse): FailureResult {
    const errorObj = httpError.error;
    if (errorObj.Code && errorObj.Message) {
      return new FailureResult(errorObj.Code, errorObj.Message);
    }
    return this.createDefaultResult();
  }

  createDefaultResult(): FailureResult {
    return FailureResult.createFromString('Es ist ein technischer Fehler aufgetreten. Bitte versuchen Sie es erneut bzw. kontaktieren Sie uns, wenn das Problem anhält.');
  }
}
