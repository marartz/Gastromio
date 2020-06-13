import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {combineLatest, Observable} from 'rxjs';
import {AuthService} from '../auth/auth.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {DishCategoryModel} from '../dish-category/dish-category.model';
import {CartModel, OrderType} from '../cart/cart.model';
import {tap} from 'rxjs/operators';

@Injectable()
export class OrderService {
  private baseUrl = 'api/v1/order';

  private restaurantId: string;

  private restaurant: RestaurantModel;
  private dishCategories: DishCategoryModel[];

  private cart: CartModel;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
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

    if (this.restaurantId && !this.restaurant) {
      console.log('initialize: push getRestaurantAsync');
      observables.push(this.getRestaurantAsync(this.restaurantId).pipe(tap(restaurant => {
        this.restaurant = restaurant;
      })));
    }

    if (this.restaurantId && !this.dishCategories) {
      console.log('initialize: push getDishesOfRestaurantAsync');
      observables.push(this.getDishesOfRestaurantAsync(this.restaurantId).pipe(tap(dishCategories => {
        this.dishCategories = dishCategories;
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
    if (this.restaurantId !== restaurantId) {
      this.restaurant = undefined;
      this.dishCategories = undefined;
      this.cart = undefined;
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
        this.cart = new CartModel(orderType,
          this.restaurant.id,
          this.restaurant.pickupInfo.averageTime,
          this.restaurant.pickupInfo.minimumOrderValue,
          this.restaurant.pickupInfo.maximumOrderValue,
          undefined,
          this.restaurant.hygienicHandling);
        break;
      case OrderType.Delivery:
        if (!this.restaurant.deliveryInfo || !this.restaurant.deliveryInfo.enabled) {
          throw new Error('restaurant does not support delivery');
        }
        this.cart = new CartModel(orderType,
          this.restaurant.id,
          this.restaurant.deliveryInfo.averageTime,
          this.restaurant.deliveryInfo.minimumOrderValue,
          this.restaurant.deliveryInfo.maximumOrderValue,
          this.restaurant.deliveryInfo.costs,
          this.restaurant.hygienicHandling);
        break;
      case OrderType.Reservation:
        if (!this.restaurant.reservationInfo || !this.restaurant.reservationInfo.enabled) {
          throw new Error('restaurant does not support reservation');
        }
        this.cart = new CartModel(orderType,
          this.restaurant.id,
          undefined,
          undefined,
          undefined,
          undefined,
          this.restaurant.hygienicHandling);
        break;
    }
  }

  public changeOrderType(orderType: OrderType) {
    if (!this.cart) {
      return;
    }

    switch (orderType) {
      case OrderType.Pickup:
        this.cart.changeOrderType(orderType,
          this.restaurant.pickupInfo.averageTime,
          this.restaurant.pickupInfo.minimumOrderValue,
          this.restaurant.pickupInfo.maximumOrderValue,
          undefined);
        break;
      case OrderType.Delivery:
        this.cart.changeOrderType(orderType,
          this.restaurant.deliveryInfo.averageTime,
          this.restaurant.deliveryInfo.minimumOrderValue,
          this.restaurant.deliveryInfo.maximumOrderValue,
          this.restaurant.deliveryInfo.costs);
        break;
      case OrderType.Reservation:
        this.cart.changeOrderType(orderType,
          undefined,
          undefined,
          undefined,
          undefined);
        break;
    }
  }

  public getCart(): CartModel {
    return this.cart;
  }

  public discardCart(): void {
    this.cart = undefined;
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
