import {Component, OnDestroy, OnInit} from '@angular/core';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {ActivatedRoute} from '@angular/router';

import {RestaurantModel} from '../restaurant/restaurant.model';
import {DishCategoryModel} from '../dish-category/dish-category.model';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {OrderService} from '../order/order.service';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {DishModel} from '../dish-category/dish.model';
import {OrderRestaurantOpeningHoursComponent} from '../order-restaurant-opening-hours/order-restaurant-opening-hours.component';
import {OrderRestaurantImprintComponent} from '../order-restaurant-imprint/order-restaurant-imprint.component';
import {CartModel, OrderType} from '../cart/cart.model';
import {OrderedDishModel} from '../cart/ordered-dish.model';
import {DishProductInfoComponent} from '../dish-productinfo/dish-productinfo.component';
import {AddDishToCartComponent} from '../add-dish-to-cart/add-dish-to-cart.component';
import {EditOrderedDishComponent} from '../edit-ordered-dish/edit-ordered-dish.component';
import {take} from 'rxjs/operators';

@Component({
  selector: 'app-order-restaurant',
  templateUrl: './order-restaurant.component.html',
  styleUrls: ['./order-restaurant.component.css', '../../assets/css/frontend.min.css']
})
export class OrderRestaurantComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  generalError: string;

  initialized: boolean;

  restaurantId: string;
  restaurant: RestaurantModel;
  imgUrl: any;
  openingHours: string;
  dishCategories: DishCategoryModel[];

  currentDishCategoryDivId: string;

  constructor(
    private route: ActivatedRoute,
    private orderService: OrderService,
    private httpErrorHandlingService: HttpErrorHandlingService,
    private modalService: NgbModal,
  ) {
  }

  ngOnInit() {
    this.initialized = false;

    this.blockUI.start('Restaurant wird geladen ...');
    this.route.paramMap.pipe(take(1)).subscribe(params => {
      this.restaurantId = params.get('restaurantId');

      const orderTypeText = params.get('orderType')?.toLocaleLowerCase();
      let orderType: OrderType;
      if (!orderTypeText) {
        orderType = OrderType.Pickup;
      } else {
        switch (orderTypeText) {
          case 'pickup':
            orderType = OrderType.Pickup;
            break;
          case 'delivery':
            orderType = OrderType.Delivery;
            break;
          case 'reservation':
            orderType = OrderType.Reservation;
            break;
          default:
            this.blockUI.stop();
            this.generalError = 'Unbekannte Bestellart: ' + orderTypeText;
        }
      }

      this.orderService.selectRestaurantAsync(this.restaurantId).pipe(take(1)).subscribe(() => {
        this.blockUI.stop();

        this.restaurant = this.orderService.getRestaurant();
        this.dishCategories = this.orderService.getDishCategories();
        this.imgUrl = this.restaurant.image;
        this.openingHours = 'Mo. 10:00-14:00';

        const cart = this.orderService.getCart();

        if (!cart || cart.getOrderType() !== orderType) {
          this.orderService.startOrder(orderType);
        } else {
          this.orderService.showCart();
        }

        this.initialized = true;
      }, error => {
        this.blockUI.stop();
        this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
      });
    });
  }

  ngOnDestroy() {
  }

  onDishCategoryChange(dishCategoryDivId: string): void {
    this.currentDishCategoryDivId = dishCategoryDivId;
  }

  scrollToDishCategory(dishCategoryId: string): void {
    const element = document.querySelector('#dc' + dishCategoryId);
    if (element === null) {
      return;
    }
    element.scrollIntoView();
  }

  openProductInfoModal(dish: DishModel): void {
    const modalRef = this.modalService.open(DishProductInfoComponent, {centered: true});
    modalRef.componentInstance.dish = dish;
    modalRef.result.then(() => {
    }, () => {
    });
  }

  isCartEmpty(): boolean {
    const cart = this.orderService.getCart();
    return cart ? !cart.hasOrders() : false;
  }

  isCartVisible(): boolean {
    const cart = this.orderService.getCart();
    return cart ? cart.isVisible() : false;
  }

  showCart() {
    this.orderService.showCart();
  }

  hideCart() {
    this.orderService.hideCart();
  }

  getCart(): CartModel {
    return this.orderService.getCart();
  }

  openOpeningHoursModal(): void {
    const modalRef = this.modalService.open(OrderRestaurantOpeningHoursComponent);
    modalRef.result.then(() => {
    }, () => {
    });
  }

  openImprintModal(): void {
    const modalRef = this.modalService.open(OrderRestaurantImprintComponent);
    modalRef.result.then(() => {
    }, () => {
    });
  }

  getFirstDishVariant(dish: DishModel): string {
    if (dish === undefined || dish.variants === undefined || dish.variants.length === 0) {
      return '0,00';
    }

    return dish.variants[0].price.toLocaleString('de', {minimumFractionDigits: 2});
  }

  public onAddDishToCart(dish: DishModel): void {
    if (dish === undefined || dish.variants === undefined || dish.variants.length === 0) {
      return;
    }
    const modalRef = this.modalService.open(AddDishToCartComponent);
    modalRef.componentInstance.dish = dish;
    modalRef.result.then(() => {
    }, () => {
    });
  }

  public onEditOrderedDish(orderedDish: OrderedDishModel): void {
    const modalRef = this.modalService.open(EditOrderedDishComponent);
    modalRef.componentInstance.orderedDish = orderedDish;
    modalRef.result.then(() => {
    }, () => {
    });
  }

  public onIncrementDishVariantCount(orderedDishVariant: OrderedDishModel): void {
    if (orderedDishVariant === undefined) {
      return;
    }
    this.orderService.incrementCountOfOrderedDish(orderedDishVariant.getItemId());
  }

  public onDecrementDishVariantCount(orderedDishVariant: OrderedDishModel): void {
    if (orderedDishVariant === undefined) {
      return;
    }
    const cart = this.orderService.getCart();
    this.orderService.decrementCountOfOrderedDish(orderedDishVariant.getItemId());
  }

  public onRemoveDishVariantFromCart(orderedDishVariant: OrderedDishModel): void {
    if (orderedDishVariant === undefined) {
      return;
    }
    const cart = this.orderService.getCart();
    this.orderService.removeOrderedDishFromCart(orderedDishVariant.getItemId());
  }
}
