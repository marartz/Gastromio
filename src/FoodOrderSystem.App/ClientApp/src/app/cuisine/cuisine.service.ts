import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CuisineModel } from './cuisine.model';

@Injectable()
export class CuisineService {
  private baseUrl: string = "api/v1";

  constructor(
    private http: HttpClient,
  ) { }

  public getAllCuisinesAsync(): Observable<CuisineModel[]> {
    let httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      })
    };
    return this.http.get<CuisineModel[]>(this.baseUrl + '/cuisines', httpOptions);
  }
}
