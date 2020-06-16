import {StoredCartDishModel} from './stored-cart-dish.model';

export class StoredCartModel {
  public orderType: string;
  public restaurantId: string;
  public cartDishes: StoredCartDishModel[];
}
