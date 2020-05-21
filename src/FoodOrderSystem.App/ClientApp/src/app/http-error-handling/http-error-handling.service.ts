import { Injectable } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlingService {

  constructor() { }

  handleError(error: HttpErrorResponse): string {
    if (typeof error.error === "string")
      return error.error;
    else
      return "Huch, das hätte nicht passieren sollen! Bitte versuchen Sie es nochmals bzw. kontaktieren Sie uns, wenn das Problem anhält.";
  }
}
