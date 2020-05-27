import { Component, OnInit, OnDestroy } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { ActivatedRoute } from '@angular/router';

import { RestaurantModel } from '../restaurant/restaurant.model';
import { HttpErrorResponse } from '@angular/common/http';
import { DishCategoryModel } from '../dish-category/dish-category.model';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { OrderService } from '../order/order.service';

import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { DishModel } from '../dish-category/dish.model';
import { OrderRestaurantOpeningHoursComponent } from '../order-restaurant-opening-hours/order-restaurant-opening-hours.component';
import { OrderRestaurantImprintComponent } from '../order-restaurant-imprint/order-restaurant-imprint.component';
import { CartModel } from '../cart/cart.model';
import { DishVariantModel } from '../dish-category/dish-variant.model';
import { OrderedDishModel } from '../cart/ordered-dish.model';
import { DishProductInfoComponent } from '../dish-productinfo/dish-productinfo.component';

@Component({
  selector: 'app-order-restaurant',
  templateUrl: './order-restaurant.component.html',
  styleUrls: ['./order-restaurant.component.css', './order-restaurant-cart.component.css']
})
export class OrderRestaurantComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  initialized: boolean = false;

  restaurantId: string;
  restaurant: RestaurantModel;
  dishCategories: DishCategoryModel[];

  generalError: string;

  imgUrl: any;

  openingHours: string;

  currentDishCategoryDivId: string;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private httpErrorHandlingService: HttpErrorHandlingService,
    private modalService: NgbModal,
  ) {
  }

  ngOnInit() {
    this.blockUI.start("Restaurant wird geladen ...");
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

          this.openingHours = "Mo. 10:00-14:00";

          let getDishesSubscription = this.orderService.getDishesOfRestaurantAsync(this.restaurantId).subscribe(
            (dishCategories) => {
              getDishesSubscription.unsubscribe();
              this.blockUI.stop();
              this.dishCategories = dishCategories;
              this.orderService.startOrderAtRestaurant(this.restaurant);
              this.initialized = true;
            },
            (error: HttpErrorResponse) => {
              getDishesSubscription.unsubscribe();
              this.blockUI.stop();
              this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
            }
          );

        },
        (error: HttpErrorResponse) => {
          getRestaurantSubscription.unsubscribe();
          this.blockUI.stop();
          this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
        }
      );
    });
  }

  ngOnDestroy() {
  }

  onDishCategoryChange(dishCategoryDivId: string): void {
    this.currentDishCategoryDivId = dishCategoryDivId;
  }

  scrollToDishCategory(dishCategoryId: string): void {
    let element = document.querySelector('#dc' + dishCategoryId);
    if (element === null)
      return;
    element.scrollIntoView();
  }

  openProductInfoModal(dish: DishModel): void {
    let modalRef = this.modalService.open(DishProductInfoComponent, { centered: true });
    modalRef.componentInstance.dish = dish;
    modalRef.result.then(() => {
    }, () => { });
  }

  isCartEmpty(): boolean {
    let cart = this.orderService.getCart();
    return cart === undefined || cart.orderedDishes === undefined || cart.orderedDishes.length === 0;
  }

  getCart(): CartModel {
    return this.orderService.getCart();
  }

  openOpeningHoursModal(): void {
    let modalRef = this.modalService.open(OrderRestaurantOpeningHoursComponent);
    modalRef.result.then(() => {
    }, () => { });
  }

  openImprintModal(): void {
    let modalRef = this.modalService.open(OrderRestaurantImprintComponent);
    modalRef.result.then(() => {
    }, () => { });
  }

  getFirstDishVariant(dish: DishModel): string {
    if (dish === undefined || dish.variants === undefined || dish.variants.length === 0)
      return "0,00";

    return dish.variants[0].price.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public onAddDishToCart(dish: DishModel): void {
    if (dish === undefined || dish.variants === undefined || dish.variants.length === 0)
      return;
    this.orderService.addDishVariantToCart(dish, dish.variants[0]);
  }

  public onIncrementDishVariantCount(orderedDishVariant: OrderedDishModel): void {
    if (orderedDishVariant === undefined)
      return;
    this.orderService.incrementDishVariantCount(orderedDishVariant.itemId);
  }

  public onDecrementDishVariantCount(orderedDishVariant: OrderedDishModel): void {
    if (orderedDishVariant === undefined)
      return;
    this.orderService.decrementDishVariantCount(orderedDishVariant.itemId);
  }

  public onRemoveDishVariantFromCart(orderedDishVariant: OrderedDishModel): void {
    if (orderedDishVariant === undefined)
      return;
    this.orderService.removeDishVariantFromCart(orderedDishVariant.itemId);
  }
}
