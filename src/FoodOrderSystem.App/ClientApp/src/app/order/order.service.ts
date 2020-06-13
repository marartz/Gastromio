import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {combineLatest, Observable} from 'rxjs';
import {AuthService} from '../auth/auth.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {DishCategoryModel} from '../dish-category/dish-category.model';
import {CartModel, OrderType} from '../cart/cart.model';
import {tap} from 'rxjs/operators';
import {StoredCartModel} from '../cart/stored-cart.model';
import {StoredOrderedDishModel} from '../cart/stored-ordered-dish.model';
import {DishModel} from '../dish-category/dish.model';
import {DishVariantModel} from '../dish-category/dish-variant.model';
import {OrderedDishModel} from '../cart/ordered-dish.model';
import {Guid} from 'guid-typescript';

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

  private static translateToOrderType(orderType: string): OrderType {
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

  private static translateFromOrderType(orderType: OrderType): string {
    switch (orderType) {
      case OrderType.Pickup:
        return 'pickup';
      case OrderType.Delivery:
        return 'delivery';
      case OrderType.Reservation:
        return 'reservation';
    }
  }

  public searchForRestaurantsAsync(search: string): Observable<RestaurantModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<RestaurantModel[]>(this.baseUrl + '/restaurants?search=' + encodeURIComponent(search), httpOptions);
  }

  public initializeAsync(): Observable<unknown> {
    console.log('initialize: start');
    const observables = [];

    this.tryLoadCartFromStorage();

    if (this.restaurantId && !this.restaurant) {
      console.log('initialize: push getRestaurantAsync');
      observables.push(this.getRestaurantAsync(this.restaurantId).pipe(tap(restaurant => {
        console.log('loaded restaurant: ', restaurant);
        this.restaurant = restaurant;
      })));
    }

    if (this.restaurantId && !this.dishCategories) {
      console.log('initialize: push getDishesOfRestaurantAsync');
      observables.push(this.getDishesOfRestaurantAsync(this.restaurantId).pipe(tap(dishCategories => {
        console.log('loaded dish categories: ', dishCategories);
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
        console.log('done');
      })));
    }

    if (observables.length > 0) {
      console.log('initialize: return combineLatest');
      return combineLatest(observables);
    } else {
      console.log('initialize: return empty observable');
      return new Observable<unknown>(observer => {
        observer.next();
        observer.complete();
        return {
          unsubscribe() {
          }
        };
      });
    }
  }

  public selectRestaurantAsync(restaurantId: string): Observable<unknown> {
    if (!this.storedCart || this.restaurantId !== restaurantId) {
      this.restaurant = undefined;
      this.dishCategories = undefined;
      this.storedCart = undefined;
    }

    this.restaurantId = restaurantId;

    return this.initializeAsync();
  }

  public getRestaurant(): RestaurantModel {
    return this.restaurant;
  }

  public getDishCategories(): DishCategoryModel[] {
    return this.dishCategories;
  }

  public startOrder(orderType: OrderType): void {
    switch (orderType) {
      case OrderType.Pickup:
        if (!this.restaurant.pickupInfo || !this.restaurant.pickupInfo.enabled) {
          throw new Error('restaurant does not support pickup');
        }
        break;
      case OrderType.Delivery:
        if (!this.restaurant.deliveryInfo || !this.restaurant.deliveryInfo.enabled) {
          throw new Error('restaurant does not support delivery');
        }
        break;
      case OrderType.Reservation:
        if (!this.restaurant.reservationInfo || !this.restaurant.reservationInfo.enabled) {
          throw new Error('restaurant does not support reservation');
        }
        break;
    }

    this.storedCart = new StoredCartModel();
    this.storedCart.orderType = OrderService.translateFromOrderType(orderType);
    this.storedCart.restaurantId = this.restaurant.id;
    this.storedCart.orderedDishes = new Array<StoredOrderedDishModel>();
  }

  public changeOrderType(orderType: OrderType) {
    if (!this.storedCart) {
      return;
    }
    this.storedCart.orderType = OrderService.translateFromOrderType(orderType);
  }

  public getCart(): CartModel {
    if (!this.storedCart || !this.restaurant || !this.dishCategories) {
      return undefined;
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

    const orderedDishes = new Array<OrderedDishModel>();
    for (const storedOrderedDish of this.storedCart.orderedDishes) {
      const dish = this.dishes.get(storedOrderedDish.dishId);
      if (!dish) {
        throw new Error('dish with id ' + storedOrderedDish.dishId + ' not known');
      }

      const variant = dish.variants.find(en => en.variantId === storedOrderedDish.variantId);
      if (!variant) {
        throw new Error('variant with id ' + storedOrderedDish.variantId + ' not known');
      }

      const orderedDish = new OrderedDishModel(
        storedOrderedDish.itemId,
        dish,
        variant,
        storedOrderedDish.count,
        storedOrderedDish.remarks
      );

      orderedDishes.push(orderedDish);
    }

    return new CartModel(
      OrderService.translateToOrderType(this.storedCart.orderType),
      this.restaurantId,
      averageTime,
      minimumOrderValue,
      maximumOrderValue,
      costs,
      this.restaurant.hygienicHandling,
      orderedDishes,
      this.visible
    );
  }

  public addDishToCart(dish: DishModel, variant: DishVariantModel, count: number): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    if (count <= 0) {
      return;
    }
    let storedOrderedDish = this.storedCart.orderedDishes.find(en => en.dishId === dish.id && en.variantId === variant.variantId);
    if (storedOrderedDish !== undefined) {
      storedOrderedDish.count += count;
    } else {
      storedOrderedDish = new StoredOrderedDishModel();
      storedOrderedDish.itemId = Guid.create().toString();
      storedOrderedDish.dishId = dish.id;
      storedOrderedDish.variantId = variant.variantId;
      storedOrderedDish.count = count;
      storedOrderedDish.remarks = undefined;
      this.storedCart.orderedDishes.push(storedOrderedDish);
    }
    this.visible = true;
    this.saveCartToStorage();
  }

  public setCountOfOrderedDish(itemId: string, count: number): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    const index = this.storedCart.orderedDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.storedCart.orderedDishes[index].count = count;
    if (this.storedCart.orderedDishes[index].count <= 0) {
      this.storedCart.orderedDishes.splice(index, 1);
    }
    this.saveCartToStorage();
  }

  public incrementCountOfOrderedDish(itemId: string): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    const index = this.storedCart.orderedDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.storedCart.orderedDishes[index].count += 1;
    this.saveCartToStorage();
  }

  public decrementCountOfOrderedDish(itemId: string): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    const index = this.storedCart.orderedDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.storedCart.orderedDishes[index].count -= 1;
    if (this.storedCart.orderedDishes[index].count <= 0) {
      this.storedCart.orderedDishes.splice(index, 1);
    }
    this.saveCartToStorage();
  }

  public changeRemarksOfOrderedDish(itemId: string, remarks: string): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    const index = this.storedCart.orderedDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.storedCart.orderedDishes[index].remarks = remarks;
    this.saveCartToStorage();
  }

  public removeOrderedDishFromCart(itemId: string): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    const index = this.storedCart.orderedDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.storedCart.orderedDishes.splice(index, 1);
    this.saveCartToStorage();
  }

  public discardCart(): void {
    this.storedCart = undefined;
    this.saveCartToStorage();
  }

  public showCart(): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    this.visible = true;
  }

  public hideCart(): void {
    if (!this.storedCart) {
      throw new Error('no cart defined');
    }
    this.visible = false;
  }

  private tryLoadCartFromStorage(): boolean {
    const json = localStorage.getItem('cart');
    if (!json) {
      return false;
    }

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
          console.log('unknown order type: ', storedCart.orderType);
          return false;
      }

      if (storedCart.restaurantId === undefined) {
        console.log('restaurant id is not available');
        return false;
      }

      if (!storedCart.orderedDishes) {
        console.log('no ordered dishes available');
        return false;
      }

      const knownItemIds = new Map<string, string>();
      for (const storedOrderedDishModel of storedCart.orderedDishes) {
        if (!storedOrderedDishModel.itemId || storedOrderedDishModel.itemId.length === 0) {
          console.log('item id is not valid:', storedOrderedDishModel.itemId);
          return false;
        }
        if (knownItemIds.get(storedOrderedDishModel.itemId)) {
          console.log('item id is not unique:', storedOrderedDishModel.itemId);
          return false;
        }
        if (!storedOrderedDishModel.dishId || storedOrderedDishModel.dishId.length === 0) {
          console.log('dish id is not valid:', storedOrderedDishModel.dishId);
          return false;
        }
        if (!storedOrderedDishModel.variantId || storedOrderedDishModel.variantId.length === 0) {
          console.log('dish id is not valid:', storedOrderedDishModel.variantId);
          return false;
        }
        if (storedOrderedDishModel.count <= 0) {
          console.log('invalid count: ', storedOrderedDishModel.count);
          return false;
        }
      }

      this.restaurantId = storedCart.restaurantId;
      this.storedCart = storedCart;

      return true;
    } catch (exc) {
      console.log('Exception in tryLoadCartFromStorage:', exc);
      return false;
    }
  }

  private saveCartToStorage(): void {
    if (!this.storedCart) {
      localStorage.removeItem('cart');
      return;
    }
    const json = JSON.stringify(this.storedCart);
    localStorage.setItem('cart', json);
  }

  private getRestaurantAsync(id: string): Observable<RestaurantModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<RestaurantModel>(this.baseUrl + '/restaurants/' + encodeURIComponent(id), httpOptions);
  }

  private getDishesOfRestaurantAsync(id: string): Observable<DishCategoryModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<DishCategoryModel[]>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/dishes', httpOptions);
  }
}
