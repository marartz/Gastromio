import { StoredCartDishModel } from './stored-cart-dish.model';
import { OrderedDishInfoModel } from './ordered-dish-info.model';
import { CartDishModel } from './cart-dish.model';

export class CheckoutModel {
  constructor(init?: Partial<CheckoutModel>) {
    if (init) {
      Object.assign(this, init);
    }
    this.cartDishes = this.cartDishes?.map((dish) => new StoredCartDishModel(dish));
  }

  public givenName: string;

  public lastName: string;

  public street: string;

  public addAddressInfo: string;

  public zipCode: string;

  public city: string;

  public phone: string;

  public email: string;

  public orderType: string;

  public restaurantId: string;

  public cartDishes: StoredCartDishModel[];

  public comments: string;

  public paymentMethodId: string;

  public serviceTime: string;

  public clone(): CheckoutModel {
    return new CheckoutModel({
      givenName: this.givenName,
      lastName: this.lastName,
      street: this.street,
      addAddressInfo: this.addAddressInfo,
      zipCode: this.zipCode,
      city: this.city,
      phone: this.phone,
      email: this.email,
      orderType: this.orderType,
      restaurantId: this.restaurantId,
      cartDishes: this.cartDishes?.map((dish) => dish?.clone()),
      comments: this.comments,
      paymentMethodId: this.paymentMethodId,
      serviceTime: this.serviceTime,
    });
  }
}
