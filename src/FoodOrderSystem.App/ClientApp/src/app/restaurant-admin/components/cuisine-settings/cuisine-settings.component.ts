import { Component, OnInit } from '@angular/core';

import {Observable} from "rxjs";
import {map} from "rxjs/operators";

import {CuisineModel} from "../../../shared/models/cuisine.model";

import {RestaurantAdminFacade} from "../../restaurant-admin.facade";

@Component({
  selector: 'app-cuisine-settings',
  templateUrl: './cuisine-settings.component.html',
  styleUrls: [
    './cuisine-settings.component.css',
    '../../../../assets/css/frontend_v2.min.css',
    '../../../../assets/css/backend_v2.min.css'
  ]
})
export class CuisineSettingsComponent implements OnInit {

  cuisines$: Observable<CuisineModel[]>;

  constructor(
    private facade: RestaurantAdminFacade
  ) {
  }

  ngOnInit(): void {
    this.cuisines$ = this.facade.getCuisines$();
  }

  isCuisineEnabled$(cuisineId: string): Observable<boolean> {
    return this.facade.getCuisineStatus$()
      .pipe(
        map(status => {
          return status[cuisineId];
        })
      );
  }

  onToggleCuisine(cuisineId: string): void {
    this.facade.toggleCuisine(cuisineId);
  }

}
