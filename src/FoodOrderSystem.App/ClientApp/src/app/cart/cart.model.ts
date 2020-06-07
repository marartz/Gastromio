import { RestaurantModel } from '../restaurant/restaurant.model';
import { OrderedDishModel } from './ordered-dish.model';
import { Guid } from 'guid-typescript';

export class CartModel {
  constructor() {
    this.orderId = Guid.create().toString();
  }

  public orderId: string;

  public orderType: OrderType;

  public restaurant: RestaurantModel;

  public orderedDishes: OrderedDishModel[];

  public getDishCountOfOrder(): number {
    let result = 0;
    for (const orderedDish of this.orderedDishes) {
      result += orderedDish.count;
    }
    return result;
  }

  public getValueOfOrder(): number {
    if (this.orderedDishes === undefined || this.orderedDishes.length === 0) {
      return 0;
    }
    let result = 0;
    for (const orderedDish of this.orderedDishes) {
      result += orderedDish.count * orderedDish.variant.price;
    }
    return result;
  }

  public getValueOfOrderText(): string {
    const val = this.getValueOfOrder();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public getAverageTime(): number {
    switch (this.orderType) {
      case OrderType.Pickup:
        return this.restaurant.pickupInfo.averageTime;
      case OrderType.Delivery:
        return this.restaurant.deliveryInfo.averageTime;
      default:
        return 0;
    }
  }

  public getMinimumOrderValue(): number {
    switch (this.orderType) {
      case OrderType.Pickup:
        return this.restaurant.pickupInfo.minimumOrderValue;
      case OrderType.Delivery:
        return this.restaurant.deliveryInfo.minimumOrderValue;
      default:
        return 0;
    }
  }

  public getMinimumOrderValueText(): string {
    const val = this.getMinimumOrderValue();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public getMaximumOrderValue(): number {
    switch (this.orderType) {
      case OrderType.Pickup:
        return this.restaurant.pickupInfo.maximumOrderValue;
      case OrderType.Delivery:
        return this.restaurant.deliveryInfo.maximumOrderValue;
      default:
        return Number.MAX_VALUE;
    }
  }

  public getMaximumOrderValueText(): string {
    const val = this.getMinimumOrderValue();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public getMissingValueToMinimum(): number {
    let val = this.getMinimumOrderValue() - this.getValueOfOrder();
    if (val < 0) {
      val = 0;
    }
    return val;
  }

  public getMissingValueToMinimumText(): string {
    const val = this.getMissingValueToMinimum();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public isDeliveryForFree(): boolean {
    return this.restaurant.deliveryInfo.costs === 0;
  }

  public getDeliveryCosts(): number {
    return this.restaurant.deliveryInfo.costs;
  }

  public getDeliveryCostsText(): string {
    const val = this.getDeliveryCosts();
    if (val > 0) {
      return val.toLocaleString('de', {minimumFractionDigits: 2});
    } else {
      return 'Gratis';
    }
  }

  public getTotalPrice(): number {
    return this.getValueOfOrder() + this.getDeliveryCosts();
  }

  public getTotalPriceText(): string {
    const val = this.getTotalPrice();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }
}

export enum OrderType {
  Pickup,
  Delivery,
  Reservation
}
