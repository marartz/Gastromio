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
    let componentErrors = Object.assign({}, errorObj.errors || {});

    // fix type
    if (typeof componentErrors !== 'object') {
      componentErrors = {};
    }

    // only return specific failure result if at least one message exists
    if (!!Object.keys(componentErrors).length) {
      const generalErrors = componentErrors[''] || [];
      delete componentErrors[''];
      return new FailureResult(generalErrors, componentErrors);
    }

    return this.createDefaultResult();
  }

  createDefaultResult(): FailureResult {
    return FailureResult.createFromString('Es ist ein technischer Fehler aufgetreten. Bitte versuchen Sie es erneut bzw. kontaktieren Sie uns, wenn das Problem anhält.');
  }
}
