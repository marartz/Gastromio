import {Component, OnInit, Input} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {RestaurantModel} from '../restaurant/restaurant.model';

@Component({
  selector: 'app-order-restaurant-opening-hours',
  templateUrl: './order-restaurant-opening-hours.component.html',
  styleUrls: ['./order-restaurant-opening-hours.component.css']
})
export class OrderRestaurantOpeningHoursComponent implements OnInit {
  @Input() restaurant: RestaurantModel;

  constructor(
    public activeModal: NgbActiveModal
  ) {
  }

  ngOnInit() {
  }
}
