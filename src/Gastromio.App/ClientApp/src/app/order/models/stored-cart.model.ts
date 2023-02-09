import { StoredCartDishModel } from './stored-cart-dish.model';

export class StoredCartModel {
  constructor(init?: Partial<StoredCartModel>) {
    if (init) {
      Object.assign(this, init);
    }
    this.cartDishes = this.cartDishes?.map((dish) => new StoredCartDishModel(dish));
  }

  public orderType: string;

  public restaurantId: string;

  public cartDishes: StoredCartDishModel[];

  public serviceTime: string;

  public clone(): StoredCartModel {
    return new StoredCartModel({
      orderType: this.orderType,
      restaurantId: this.restaurantId,
      cartDishes: this.cartDishes?.map((dish) => dish?.clone()),
      serviceTime: this.serviceTime,
    });
  }
}
