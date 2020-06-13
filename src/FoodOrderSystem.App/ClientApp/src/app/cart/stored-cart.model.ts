import {StoredOrderedDishModel} from './stored-ordered-dish.model';

export class StoredCartModel
{
  public orderType: string;
  public restaurantId: string;
  public orderedDishes: StoredOrderedDishModel[];
}
