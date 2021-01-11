import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';

import {combineLatest, Observable} from 'rxjs';
import {take, tap} from 'rxjs/operators';

import {Guid} from 'guid-typescript';

import {AuthService} from '../../auth/services/auth.service';

import {RestaurantModel} from '../../shared/models/restaurant.model';
import {CuisineModel} from '../../shared/models/cuisine.model';
import {DishCategoryModel} from '../../shared/models/dish-category.model';
import {DishModel} from '../../shared/models/dish.model';
import {DishVariantModel} from '../../shared/models/dish-variant.model';

import {CartModel, OrderType} from '../models/cart.model';
import {StoredCartModel} from '../models/stored-cart.model';
import {CheckoutModel} from '../models/checkout.model';
import {OrderModel} from '../models/order.model';
import {StoredCartDishModel} from '../models/stored-cart-dish.model';
import {CartDishModel} from '../models/cart-dish.model';

@Injectable()
export class OrderService {

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
  }

  private baseUrl = 'api/v1/order';

  private restaurantId: string;
  private restaurant: RestaurantModel;
  private dishCategories: DishCategoryModel[];
  private dishes: Map<string, DishModel>;
  private visible: boolean;

  private storedCart: StoredCartModel;

  private cart: CartModel;

  private order: OrderModel;

  public static translateToOrderType(orderType: string): OrderType {
    switch (orderType) {
      case 'pickup':
        return OrderType.Pickup;
      case 'delivery':
        return OrderType.Delivery;
      case 'reservation':
        return OrderType.Reservation;
      default:
        throw new Error('unknown order type: ' + orderType);
    }
  }

  public static translateFromOrderType(orderType: OrderType): string {
    switch (orderType) {
      case OrderType.Pickup:
        return 'pickup';
      case OrderType.Delivery:
        return 'delivery';
      case OrderType.Reservation:
        return 'reservation';
    }
  }

  public getAllCuisinesAsync(): Observable<CuisineModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };

    return this.http.get<RestaurantModel[]>(this.baseUrl + '/cuisines', httpOptions)
      .pipe(take(1));
  }

  public searchForRestaurantsAsync(search: string, orderType: OrderType, cuisineId: string,
                                   openingHourFilter: string | undefined): Observable<RestaurantModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };

    let orderTypeText: string;
    switch (orderType) {
      case OrderType.Pickup:
        orderTypeText = 'pickup';
        break;
      case OrderType.Delivery:
        orderTypeText = 'delivery';
        break;
      case OrderType.Reservation:
        orderTypeText = 'reservation';
    }

    let url = this.baseUrl + '/restaurants?search=' + encodeURIComponent(search);

    if (orderType != undefined) {
      url += '&orderType=' + encodeURIComponent(orderTypeText);
    }

    if (cuisineId) {
      url += '&cuisineId=' + encodeURIComponent(cuisineId);
    }

    if (openingHourFilter !== undefined) {
      const date = Date.parse(openingHourFilter);
      if (!isNaN(date)) {
        url += '&openingHour=' + encodeURIComponent(openingHourFilter);
      }
    }

    return this.http.get<RestaurantModel[]>(url, httpOptions)
      .pipe(take(1));
  }

  public initializeAsync(): Observable<unknown> {
    this.tryLoadCartFromStorage();
    return this.loadDataAsync();
  }

  public selectRestaurantAsync(restaurantId: string): Observable<unknown> {
    this.tryLoadCartFromStorage();
    if (!this.storedCart || this.restaurantId !== restaurantId) {
      this.restaurant = undefined;
      this.dishCategories = undefined;
      this.storedCart = undefined;
      this.dishes = undefined;
      this.visible = false;
      this.saveCartToStorage();
    }
    this.restaurantId = restaurantId;
    this.generateCartModel();
    return this.loadDataAsync();
  }

  public getRestaurant(): RestaurantModel {
    return this.restaurant;
  }

  public getDishCategories(): DishCategoryModel[] {
    return this.dishCategories;
  }

  public startOrder(orderType: OrderType, serviceTime: Date): void {
    switch (orderType) {
      case OrderType.Pickup:
        if (!this.restaurant.pickupInfo || !this.restaurant.pickupInfo.enabled) {
          throw new Error('Entschuldigung, die Bestellart Abholung wird von dem Restaurant nicht unterstützt');
        }
        break;
      case OrderType.Delivery:
        if (!this.restaurant.deliveryInfo || !this.restaurant.deliveryInfo.enabled) {
          throw new Error('Entschuldigung, die Bestellart Lieferung wird von dem Restaurant nicht unterstützt');
        }
        break;
      case OrderType.Reservation:
        if (!this.restaurant.reservationInfo || !this.restaurant.reservationInfo.enabled) {
          throw new Error('Entschuldigung, die Bestellart Reservierung wird von dem Restaurant nicht unterstützt');
        }
        break;
    }

    this.storedCart = new StoredCartModel();
    this.storedCart.orderType = OrderService.translateFromOrderType(orderType);
    this.storedCart.restaurantId = this.restaurant.alias;
    this.storedCart.cartDishes = new Array<StoredCartDishModel>();
    this.storedCart.serviceTime = serviceTime?.toISOString();
    this.generateCartModel();
  }

  public changeOrderType(orderType: OrderType) {
    if (!this.storedCart) {
      return;
    }
    this.storedCart.orderType = OrderService.translateFromOrderType(orderType);
    this.generateCartModel();
  }

  public getCart(): CartModel {
    return this.cart;
  }

  public addDishToCart(dish: DishModel, variant: DishVariantModel, count: number, remarks: string): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    if (count <= 0) {
      return;
    }
    let storedCartDish = this.storedCart.cartDishes.find(en => en.dishId === dish.id && en.variantId === variant.variantId);
    if (storedCartDish !== undefined) {
      storedCartDish.count += count;
      storedCartDish.remarks = remarks;
    } else {
      storedCartDish = new StoredCartDishModel();
      storedCartDish.itemId = Guid.create().toString();
      storedCartDish.dishId = dish.id;
      storedCartDish.variantId = variant.variantId;
      storedCartDish.count = count;
      storedCartDish.remarks = remarks;
      this.storedCart.cartDishes.push(storedCartDish);
    }
    this.visible = true;
    this.saveCartToStorage();
    this.generateCartModel();
  }

  public setCountOfCartDish(itemId: string, count: number): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    const index = this.storedCart.cartDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.storedCart.cartDishes[index].count = count;
    if (this.storedCart.cartDishes[index].count <= 0) {
      this.storedCart.cartDishes.splice(index, 1);
    }
    this.saveCartToStorage();
    this.generateCartModel();
  }

  public incrementCountOfCartDish(itemId: string): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    const index = this.storedCart.cartDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.storedCart.cartDishes[index].count += 1;
    this.saveCartToStorage();
    this.generateCartModel();
  }

  public decrementCountOfCartDish(itemId: string): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    const index = this.storedCart.cartDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.storedCart.cartDishes[index].count -= 1;
    if (this.storedCart.cartDishes[index].count <= 0) {
      this.storedCart.cartDishes.splice(index, 1);
    }
    this.saveCartToStorage();
    this.generateCartModel();
  }

  public changeRemarksOfCartDish(itemId: string, remarks: string): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    const index = this.storedCart.cartDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.storedCart.cartDishes[index].remarks = remarks;
    this.saveCartToStorage();
    this.generateCartModel();
  }

  public removeCartDishFromCart(itemId: string): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    const index = this.storedCart.cartDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.storedCart.cartDishes.splice(index, 1);
    this.saveCartToStorage();
    this.generateCartModel();
  }

  public discardCart(): void {
    this.storedCart = undefined;
    this.saveCartToStorage();
    this.generateCartModel();
  }

  public showCart(): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    this.visible = true;
    this.generateCartModel();
  }

  public hideCart(): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    this.visible = false;
    this.generateCartModel();
  }

  public checkoutAsync(checkoutModel: CheckoutModel): Observable<OrderModel> {
    return this.sendCheckoutAsync(checkoutModel)
      .pipe(tap(o => {
        this.order = o;
        this.discardCart();
      }));
  }

  public getOrder(): OrderModel {
    return this.order;
  }


  /* private methods */

  private tryLoadCartFromStorage(): boolean {
    const json = localStorage.getItem('cart');
    if (!json) {
      return false;
    }
    console.log('loaded cart from storage: ', json);

    try {
      const storedCart = new StoredCartModel();
      const tempObj = JSON.parse(json);
      Object.assign(storedCart, tempObj);

      switch (storedCart.orderType) {
        case 'pickup':
          break;
        case 'delivery':
          break;
        case 'reservation':
          break;
        default:
          return false;
      }

      if (storedCart.restaurantId === undefined) {
        return false;
      }

      if (!storedCart.cartDishes) {
        return false;
      }

      const knownItemIds = new Map<string, string>();
      for (const storedCartDishModel of storedCart.cartDishes) {
        if (!storedCartDishModel.itemId || storedCartDishModel.itemId.length === 0) {
          return false;
        }
        if (knownItemIds.get(storedCartDishModel.itemId)) {
          return false;
        }
        if (!storedCartDishModel.dishId || storedCartDishModel.dishId.length === 0) {
          return false;
        }
        if (!storedCartDishModel.variantId || storedCartDishModel.variantId.length === 0) {
          return false;
        }
        if (storedCartDishModel.count <= 0) {
          return false;
        }
      }

      this.restaurantId = storedCart.restaurantId;
      this.storedCart = storedCart;
      this.generateCartModel();
      return true;
    } catch (exc) {
      return false;
    }
  }

  private loadDataAsync(): Observable<unknown> {
    const observables = [];

    if (this.restaurantId && !this.restaurant) {
      observables.push(this.getRestaurantAsync(this.restaurantId).pipe(tap(restaurant => {
        this.restaurant = new RestaurantModel(restaurant);
      })));
    }

    if (this.restaurantId && !this.dishCategories) {
      observables.push(this.getDishesOfRestaurantAsync(this.restaurantId).pipe(tap(dishCategories => {
        this.dishCategories = dishCategories;
        this.dishes = new Map<string, DishModel>();
        for (const dishCategory of dishCategories) {
          if (!dishCategory.dishes) {
            continue;
          }
          for (const dish of dishCategory.dishes) {
            this.dishes.set(dish.id, dish);
          }
        }
      })));
    }

    if (observables.length > 0) {
      return combineLatest(observables).pipe(tap(() => {
        this.generateCartModel();
      }));
    } else {
      return new Observable<unknown>(observer => {
        this.generateCartModel();
        observer.next();
        observer.complete();
        return {
          unsubscribe() {
          }
        };
      });
    }
  }

  private saveCartToStorage(): void {
    if (!this.storedCart) {
      localStorage.removeItem('cart');
      return;
    }
    const json = JSON.stringify(this.storedCart);
    console.log('stored cart to storage: ', json);
    localStorage.setItem('cart', json);
  }

  private generateCartModel(): void {
    if (!this.storedCart || !this.restaurant || !this.dishCategories) {
      this.cart = undefined;
      return;
    }

    let averageTime: number;
    let minimumOrderValue: number;
    let maximumOrderValue: number;
    let costs: number;

    switch (this.storedCart.orderType) {
      case 'pickup':
        averageTime = this.restaurant.pickupInfo.averageTime;
        minimumOrderValue = this.restaurant.pickupInfo.minimumOrderValue;
        maximumOrderValue = this.restaurant.pickupInfo.maximumOrderValue;
        costs = undefined;
        break;
      case 'delivery':
        averageTime = this.restaurant.deliveryInfo.averageTime;
        minimumOrderValue = this.restaurant.deliveryInfo.minimumOrderValue;
        maximumOrderValue = this.restaurant.deliveryInfo.maximumOrderValue;
        costs = this.restaurant.deliveryInfo.costs;
        break;
      case 'reservation':
        averageTime = undefined;
        minimumOrderValue = undefined;
        maximumOrderValue = undefined;
        costs = undefined;
        break;
    }

    const cartDishes = new Array<CartDishModel>();
    for (const storedCartDish of this.storedCart.cartDishes) {
      const dish = this.dishes.get(storedCartDish.dishId);
      if (!dish) {
        throw new Error('dish with id ' + storedCartDish.dishId + ' not known');
      }

      const variant = dish.variants.find(en => en.variantId === storedCartDish.variantId);
      if (!variant) {
        throw new Error('variant with id ' + storedCartDish.variantId + ' not known');
      }

      const CartDish = new CartDishModel(
        storedCartDish.itemId,
        dish,
        variant,
        storedCartDish.count,
        storedCartDish.remarks
      );

      cartDishes.push(CartDish);
    }

    let serviceTime: Date = undefined;
    try {
      const dt = this.storedCart.serviceTime.split(/[: T-]/).map(parseFloat);
      serviceTime = new Date(Date.UTC(dt[0], dt[1] - 1, dt[2], dt[3] || 0, dt[4] || 0, dt[5] || 0, 0));
    }
    catch {}


    this.cart = new CartModel(
      OrderService.translateToOrderType(this.storedCart.orderType),
      this.restaurantId,
      averageTime,
      minimumOrderValue,
      maximumOrderValue,
      costs,
      this.restaurant.hygienicHandling,
      cartDishes,
      this.visible,
      serviceTime
    );
  }

  private getRestaurantAsync(id: string): Observable<RestaurantModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<RestaurantModel>(this.baseUrl + '/restaurants/' + encodeURIComponent(id), httpOptions)
      .pipe(take(1));
  }

  private getDishesOfRestaurantAsync(id: string): Observable<DishCategoryModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<DishCategoryModel[]>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/dishes', httpOptions)
      .pipe(take(1));
  }

  private sendCheckoutAsync(checkoutModel: CheckoutModel): Observable<OrderModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json'
      })
    };
    return this.http.post<OrderModel>(this.baseUrl + '/checkout', checkoutModel, httpOptions)
      .pipe(take(1));
  }
}
