import {Component, Input, OnDestroy, OnInit} from '@angular/core';
import {RestaurantModel} from '../restaurant/restaurant.model';

@Component({
  selector: 'app-order-restaurants-row',
  templateUrl: './order-restaurants-row.component.html',
  styleUrls: [
    './order-restaurants-row.component.css',
    '../../assets/css/frontend_v3.min.css'
  ]
})
export class OrderRestaurantsRowComponent implements OnInit, OnDestroy {

  @Input() restaurant: RestaurantModel;
  @Input() orderType: string;
  @Input() selectedOpeningHourFilter: Date;

  constructor() {
  }

  ngOnInit() {
  }

  ngOnDestroy() {
  }

  hasLogo(restaurant: RestaurantModel  ): boolean {
    return restaurant.imageTypes.some(en => en === 'logo');
  }

  getLogoUrl(restaurant: RestaurantModel): string {
    if (!restaurant) {
      return undefined;
    }
    return '/api/v1/restaurants/' + restaurant.id + '/images/logo';
  }

  hasBanner(restaurant: RestaurantModel): boolean {
    return restaurant.imageTypes.some(en => en === 'banner');
  }

  getBannerStyle(restaurant: RestaurantModel): string {
    if (!restaurant) {
      return undefined;
    }
    return '/api/v1/restaurants/' + restaurant.id + '/images/banner';
  }

  isRestaurantOpen(restaurant: RestaurantModel): boolean {
    const date = this.selectedOpeningHourFilter ?? new Date();
    return restaurant.isOpen(date);
  }
}
