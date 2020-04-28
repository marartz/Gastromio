import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { CuisineModel } from '../cuisine/cuisine.model';
import { CuisineService } from '../cuisine/cuisine.service';

@Component({
  selector: 'app-order-home',
  templateUrl: './order-home.component.html',
  styleUrls: ['./order-home.component.css']
})
export class OrderHomeComponent implements OnInit {
  cuisines: Observable<CuisineModel[]>;

  constructor(
    private cuisineService: CuisineService
  ) { }

  ngOnInit() {
    this.cuisines = this.cuisineService.getAllCuisinesAsync();
  }

}
