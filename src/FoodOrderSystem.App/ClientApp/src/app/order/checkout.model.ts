import {StoredCartDishModel} from '../cart/stored-cart-dish.model';

export class CheckoutModel {
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
}
