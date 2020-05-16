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

  public getValueOfOrder(): number {
    if (this.orderedDishes === undefined || this.orderedDishes.length === 0)
      return 0;
    let result = 0;
    for (let orderedDish of this.orderedDishes) {
      result += orderedDish.count * orderedDish.variant.price;
    }
    return result;
  }

  public getValueOfOrderText(): string {
    let val = this.getValueOfOrder();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public getMinimumOrderValue(): number {
    return this.restaurant.minimumOrderValue;
  }

  public getMinimumOrderValueText(): string {
    let val = this.getMinimumOrderValue();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public getMissingValueToMinimum(): number {
    let val = this.getMinimumOrderValue() - this.getValueOfOrder();
    if (val < 0)
      val = 0;
      return val;
  }

  public getMissingValueToMinimumText(): string {
    let val = this.getMissingValueToMinimum();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public isDeliveryForFree(): boolean {
    return this.restaurant.deliveryCosts === 0;
  }

  public getDeliveryCosts(): number {
    return this.restaurant.deliveryCosts;
  }

  public getDeliveryCostsText(): string {
    let val = this.getDeliveryCosts();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public getTotalPrice(): number {
    return this.getValueOfOrder() + this.getDeliveryCosts();
  }

  public getTotalPriceText(): string {
    let val = this.getTotalPrice();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }
}
