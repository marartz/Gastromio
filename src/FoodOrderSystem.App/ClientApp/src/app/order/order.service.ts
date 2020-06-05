import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {AuthService} from '../auth/auth.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {DishCategoryModel} from '../dish-category/dish-category.model';
import {CartModel} from '../cart/cart.model';
import {OrderedDishModel} from '../cart/ordered-dish.model';
import {DishVariantModel} from '../dish-category/dish-variant.model';
import {DishModel} from '../dish-category/dish.model';

@Injectable()
export class OrderService {
  private baseUrl = 'api/v1/order';

  private isCartVisibile: boolean;
  private cart: CartModel;

  constructor(
    private http: HttpClient,
    private authService: AuthService
  ) {
    this.isCartVisibile = false;
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

  public getRestaurantAsync(id: string): Observable<RestaurantModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<RestaurantModel>(this.baseUrl + '/restaurants/' + encodeURIComponent(id), httpOptions);
  }

  public getDishesOfRestaurantAsync(id: string): Observable<DishCategoryModel[]> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: 'Bearer ' + this.authService.getToken(),
      })
    };
    return this.http.get<DishCategoryModel[]>(this.baseUrl + '/restaurants/' + encodeURIComponent(id) + '/dishes', httpOptions);
  }

  public isCartVisible(): boolean {
    return this.isCartVisibile;
  }

  public showCart(): void {
    this.isCartVisibile = true;
  }

  public hideCart(): void {
    this.isCartVisibile = false;
  }

  public getCart(): CartModel {
    return this.cart;
  }

  public startOrderAtRestaurant(restaurant: RestaurantModel): void {
    this.cart = new CartModel();
    this.cart.restaurant = restaurant;
    this.cart.orderedDishes = new Array<OrderedDishModel>();
  }

  public addDishVariantToCart(dish: DishModel, variant: DishVariantModel): void {
    let orderedDish = this.cart.orderedDishes.find(en => en.dish.id === dish.id && en.variant.variantId === variant.variantId);
    if (orderedDish !== undefined) {
      orderedDish.count++;
    } else {
      orderedDish = new OrderedDishModel();
      orderedDish.dish = dish;
      orderedDish.variant = variant;
      orderedDish.count = 1;
      this.cart.orderedDishes.push(orderedDish);
    }
    this.isCartVisibile = true;
  }

  public incrementDishVariantCount(itemId: string): void {
    const index = this.cart.orderedDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.cart.orderedDishes[index].count++;
  }

  public decrementDishVariantCount(itemId: string): void {
    const index = this.cart.orderedDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.cart.orderedDishes[index].count--;
    if (this.cart.orderedDishes[index].count === 0) {
      this.cart.orderedDishes.splice(index, 1);
    }
  }

  public removeDishVariantFromCart(itemId: string): void {
    const index = this.cart.orderedDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.cart.orderedDishes.splice(index, 1);
  }

  public discardCart(): void {
    this.cart = undefined;
  }
}
