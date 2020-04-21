import { Component, OnInit } from '@angular/core';

const restaurants = [
  {
    name: 'Phone XL',
    description: 'A large phone with one of the best screens',
    nextDeliveryTime: '16:00',
    deliveryCosts: 'Gratis',
    minimumOrderValue: '10,00€',
  },
  {
    name: 'Phone Mini',
    description: 'A great phone with one of the best cameras',
    nextDeliveryTime: '16:00',
    deliveryCosts: '1,50€',
    minimumOrderValue: '10,00€',
  },
  {
    name: 'Phone Standard',
    description: '',
    nextDeliveryTime: '16:00',
    deliveryCosts: '1,00€',
    minimumOrderValue: '10,00€',
  }
];

@Component({
  selector: 'app-restaurant-search',
  templateUrl: './restaurant-search.component.html',
  styleUrls: ['./restaurant-search.component.css']
})
export class RestaurantSearchComponent implements OnInit {
  restaurants = restaurants;

  constructor() { }

  ngOnInit() {
  }

}
