import { RestaurantModel } from '../restaurant/restaurant.model';
import { OrderedDishModel } from './ordered-dish.model';
import { Guid } from 'guid-typescript';

export class CartModel {
  constructor() {
    this.orderId = Guid.create().toString();
  }

  public orderId: string;

  public restaurant: RestaurantModel;

  public orderedDishes: OrderedDishModel[];

  public getPriceOfOrder(): number {
    if (this.orderedDishes === undefined || this.orderedDishes.length === 0)
      return 0;
    let result = 0;
    for (let orderedDish of this.orderedDishes) {
      result += orderedDish.count * orderedDish.variant.price;
    }
    return result;
  }

  public getPriceOfOrderText(): string {
    let val = this.getPriceOfOrder();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public getDeliveryCosts(): number {
    return this.restaurant.deliveryCosts;
  }

  public getDeliveryCostsText(): string {
    let val = this.getDeliveryCosts();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public getTotalPrice(): number {
    return this.getPriceOfOrder() + this.getDeliveryCosts();
  }

  public getTotalPriceText(): string {
    let val = this.getTotalPrice();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }
}
