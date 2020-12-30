import {Injectable} from '@angular/core';

import {StoredCartModel} from "../models/stored-cart.model";

@Injectable()
export class StoredCartService {

  constructor(
  ) {
  }

  public loadFromStorage(): StoredCartModel {
    const json = localStorage.getItem('cart');
    if (!json) {
      return undefined;
    }
    console.log('loaded cart from storage: ', json);

    try {
      const storedCartModel = new StoredCartModel();
      const tempObj = JSON.parse(json);
      Object.assign(storedCartModel, tempObj);

      switch (storedCartModel.orderType) {
        case 'pickup':
          break;
        case 'delivery':
          break;
        case 'reservation':
          break;
        default:
          return undefined;
      }

      if (storedCartModel.restaurantId === undefined) {
        return undefined;
      }

      if (!storedCartModel.cartDishes) {
        return undefined;
      }

      const knownItemIds = new Map<string, string>();
      for (const storedCartDishModel of storedCartModel.cartDishes) {
        if (!storedCartDishModel.itemId || storedCartDishModel.itemId.length === 0) {
          return undefined;
        }
        if (knownItemIds.get(storedCartDishModel.itemId)) {
          return undefined;
        }
        if (!storedCartDishModel.dishId || storedCartDishModel.dishId.length === 0) {
          return undefined;
        }
        if (!storedCartDishModel.variantId || storedCartDishModel.variantId.length === 0) {
          return undefined;
        }
        if (storedCartDishModel.count <= 0) {
          return undefined;
        }
      }

      return storedCartModel;
    } catch (exc) {
      return undefined;
    }
  }

  public saveToStorage(storedCart: StoredCartModel): void {
    const json = JSON.stringify(storedCart);
    console.log('stored cart to storage: ', json);
    localStorage.setItem('cart', json);
  }

  public removeFromStorage(): void {
    console.log('removed cart from storage');
    localStorage.removeItem('cart');
  }

}
