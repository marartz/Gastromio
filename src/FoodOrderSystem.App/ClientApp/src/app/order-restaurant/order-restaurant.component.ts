import { Component, OnInit, OnDestroy } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { ActivatedRoute } from '@angular/router';

import { RestaurantModel } from '../restaurant/restaurant.model';
import { HttpErrorResponse } from '@angular/common/http';
import { DishCategoryModel } from '../dish-category/dish-category.model';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { OrderService } from '../order/order.service';

@Component({
  selector: 'app-order-restaurant',
  templateUrl: './order-restaurant.component.html',
  styleUrls: ['./order-restaurant.component.css']
})
export class OrderRestaurantComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  restaurantId: string;
  restaurant: RestaurantModel;
  dishCategories: DishCategoryModel[];

  generalError: string;

  imgUrl: any;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
    this.blockUI.start("Lade Restaurantdaten...");
    this.route.paramMap.subscribe(params => {
      this.restaurantId = params.get('restaurantId');

      let getRestaurantSubscription = this.orderService.getRestaurantAsync(this.restaurantId).subscribe(
        (data) => {
          getRestaurantSubscription.unsubscribe();

          this.restaurant = data;

          this.restaurant.paymentMethods.sort((a, b) => {
            if (a.name < b.name)
              return -1;
            if (a.name > b.name)
              return 1;
            return 0;
          });

          this.imgUrl = this.restaurant.image;

          let getDishesSubscription = this.orderService.getDishesOfRestaurantAsync(this.restaurantId).subscribe(
            (dishCategories) => {
              getDishesSubscription.unsubscribe();
              this.blockUI.stop();
              this.dishCategories = dishCategories;
            },
            (error: HttpErrorResponse) => {
              getDishesSubscription.unsubscribe();
              this.blockUI.stop();
              this.generalError = this.httpErrorHandlingService.handleError(error);
            }
          );

        },
        (error: HttpErrorResponse) => {
          getRestaurantSubscription.unsubscribe();
          this.blockUI.stop();
          this.generalError = this.httpErrorHandlingService.handleError(error);
        }
      );
    });
  }

  ngOnDestroy() {
  }
}
