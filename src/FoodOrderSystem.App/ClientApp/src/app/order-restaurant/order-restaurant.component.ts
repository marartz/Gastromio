import {Component, OnDestroy, OnInit} from '@angular/core';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {ActivatedRoute, Router} from '@angular/router';

import {RestaurantModel} from '../restaurant/restaurant.model';
import {DishCategoryModel} from '../dish-category/dish-category.model';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {OrderService} from '../order/order.service';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {DishModel} from '../dish-category/dish.model';
import {OrderRestaurantOpeningHoursComponent} from '../order-restaurant-opening-hours/order-restaurant-opening-hours.component';
import {OrderRestaurantImprintComponent} from '../order-restaurant-imprint/order-restaurant-imprint.component';
import {CartModel, OrderType} from '../cart/cart.model';
import {CartDishModel} from '../cart/cart-dish.model';
import {DishProductInfoComponent} from '../dish-productinfo/dish-productinfo.component';
import {AddDishToCartComponent} from '../add-dish-to-cart/add-dish-to-cart.component';
import {EditCartDishComponent} from '../edit-cart-dish/edit-cart-dish.component';
import {take, tap} from 'rxjs/operators';
import {combineLatest} from 'rxjs';
import {Title} from "@angular/platform-browser";

@Component({
  selector: 'app-order-restaurant',
  templateUrl: './order-restaurant.component.html',
  styleUrls: [
    './order-restaurant.component.css',
    '../../assets/css/frontend_v3.min.css',

    '../../assets/css/frontend_v2.min.css'
  ]
})
export class OrderRestaurantComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  url: string;

  orderType: string;

  generalError: string;

  initialized: boolean;

  restaurantId: string;
  restaurant: RestaurantModel;
  openingHours: string;
  dishCategories: DishCategoryModel[];

  searchPhrase: string;
  filteredDishCategories: DishCategoryModel[];

  currentDishCategoryDivId: string;

  proceedError: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private titleService: Title,
    private orderService: OrderService,
    private httpErrorHandlingService: HttpErrorHandlingService,
    private modalService: NgbModal,
  ) {
  }

  ngOnInit() {
    this.url = this.router.url;
    this.initialized = false;

    this.blockUI.start('Restaurant wird geladen ...');

    let orderType: OrderType;
    let serviceTime: Date;

    const observables = [
      this.route.paramMap.pipe(tap(params => {
        this.restaurantId = params.get('restaurantId');
      })),
      this.route.queryParams.pipe(tap(params => {
        const orderTypeText = params.orderType?.toLocaleLowerCase();
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

        const serviceTimeText = params.serviceTime;
        if (serviceTimeText) {
          try {
            const dt = serviceTimeText.split(/[: T-]/).map(parseFloat);
            serviceTime = new Date(Date.UTC(dt[0], dt[1] - 1, dt[2], dt[3] || 0, dt[4] || 0, dt[5] || 0, 0));
          }
          catch {}
        }
      }))
    ];

    combineLatest(observables).pipe(take(1)).subscribe(() => {
      this.orderService.selectRestaurantAsync(this.restaurantId).pipe(take(1)).subscribe(() => {
        this.blockUI.stop();

        this.restaurant = this.orderService.getRestaurant();
        this.dishCategories = this.orderService.getDishCategories();
        this.openingHours = this.restaurant.openingHoursTodayText;

        const cart = this.orderService.getCart();

        if (!cart || cart.getOrderType() !== orderType || cart.getServiceTime() != serviceTime) {
          this.orderService.startOrder(orderType, serviceTime);
        } else {
          this.orderService.showCart();
        }

        this.filterDishCategories();

        this.titleService.setTitle(this.restaurant.name + ' - Gastromio');

        this.initialized = true;
      }, error => {
        this.blockUI.stop();
        this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
      });
    });
  }

  ngOnDestroy() {
  }

  hasLogo(): boolean {
    return this.restaurant?.imageTypes.some(en => en === 'logo');
  }

  getLogoUrl(): string {
    if (!this.restaurant) {
      return undefined;
    }
    return '/api/v1/restaurants/' + this.restaurant.id + '/images/logo';
  }

  hasBanner(): boolean {
    return this.restaurant?.imageTypes.some(en => en === 'banner');
  }

  getBannerStyle(): string {
    if (!this.restaurant) {
      return undefined;
    }
    return 'url(\'/api/v1/restaurants/' + this.restaurant.id + '/images/banner' + '\')';
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

  scrollToExternalMenu(externalMenuId: string): void {
    const element = document.querySelector('#em' + externalMenuId);
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

  hideCart() {
    this.orderService.hideCart();
  }

  showCart() {
    this.orderService.showCart();
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
    modalRef.componentInstance.restaurant = this.restaurant;
    modalRef.result.then(() => {
    }, () => {
    });
  }

  onSearchTextChanged(searchPhrase: string): void {
    if (!searchPhrase || searchPhrase.trim().length === 0) {
      this.searchPhrase = undefined;
    } else {
      this.searchPhrase = searchPhrase.toLocaleLowerCase();
    }
    this.filterDishCategories();
  }

  filterDishCategories(): void {
    if (!this.searchPhrase) {
      this.filteredDishCategories = this.dishCategories;
      return;
    }

    this.filteredDishCategories = new Array<DishCategoryModel>();

    for (let dishCategory of this.dishCategories) {
      let hasMatch = false;

      let dishCategoryClone = new DishCategoryModel();
      dishCategoryClone.id = dishCategory.id;
      dishCategoryClone.name = dishCategory.name;
      dishCategoryClone.dishes = new Array<DishModel>();

      for (let dish of dishCategory.dishes) {
        const nameContainsSearchPhrase = dish.name && dish.name.toLocaleLowerCase().indexOf(this.searchPhrase) > -1;
        const descriptionContainsSearchPhrase = dish.description && dish.description.toLocaleLowerCase().indexOf(this.searchPhrase) > -1;
        if (nameContainsSearchPhrase || descriptionContainsSearchPhrase) {
          dishCategoryClone.dishes.push(dish);
          hasMatch = true;
        }
      }

      if (hasMatch) {
        this.filteredDishCategories.push(dishCategoryClone);
      }
    }
  }

  getFirstDishVariant(dish: DishModel): string {
    if (dish === undefined || dish.variants === undefined || dish.variants.length === 0) {
      return '0,00';
    }

    return dish.variants[0].price.toLocaleString('de', {minimumFractionDigits: 2});
  }

  getRestaurantSubText(restaurant: RestaurantModel): string {
    if (restaurant === undefined || restaurant.cuisines === undefined || restaurant.cuisines.length === 0) {
      return '';
    }
    return restaurant.cuisines.map(en => en.name).join(', ');
  }

  public onAddDishToCart(dish: DishModel): void {
    if (dish === undefined || dish.variants === undefined || dish.variants.length === 0) {
      return;
    }
    const modalRef = this.modalService.open(AddDishToCartComponent);
    modalRef.componentInstance.dish = dish;
    modalRef.result.then(() => {
      this.proceedError = undefined;
    }, () => {
    });
  }

  public onEditCartDish(cartDish: CartDishModel): void {
    const modalRef = this.modalService.open(EditCartDishComponent);
    modalRef.componentInstance.cartDish = cartDish;
    modalRef.result.then(() => {
      this.proceedError = undefined;
    }, () => {
    });
  }

  public onIncrementDishVariantCount(cartDishVariant: CartDishModel): void {
    if (cartDishVariant === undefined) {
      return;
    }
    this.orderService.incrementCountOfCartDish(cartDishVariant.getItemId());
    this.proceedError = undefined;
  }

  public onDecrementDishVariantCount(cartDishVariant: CartDishModel): void {
    if (cartDishVariant === undefined) {
      return;
    }
    this.orderService.decrementCountOfCartDish(cartDishVariant.getItemId());
    this.proceedError = undefined;
  }

  public onRemoveDishVariantFromCart(cartDishVariant: CartDishModel): void {
    if (cartDishVariant === undefined) {
      return;
    }
    this.orderService.removeCartDishFromCart(cartDishVariant.getItemId());
    this.proceedError = undefined;
  }

  public proceedToCheckout(): void {
    if (!this.getCart().isValid()) {
      this.proceedError = this.getCart().getValidationError();
      return;
    }
    this.proceedError = undefined;
    this.router.navigateByUrl('/checkout');
  }
}
