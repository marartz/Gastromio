import {OrderedDishModel} from './ordered-dish.model';

export class CartModel {
  constructor(
    private orderType: OrderType,
    private restaurantId: string,
    private averageTime: number,
    private minimumOrderValue: number,
    private maximumOrderValue: number,
    private costs: number,
    private hygienicHandling: string,
    private orderedDishes: OrderedDishModel[],
    private visible: boolean
  ) {
  }

  public getOrderType(): OrderType {
    return this.orderType;
  }

  public getOrderTypeText(): string {
    switch (this.getOrderType()) {
      case OrderType.Pickup:
        return 'Abholung';
      case OrderType.Delivery:
        return 'Lieferung';
      case OrderType.Reservation:
        return 'Tischreservierung';

    }
  }

  public getRestaurantId(): string {
    return this.restaurantId;
  }

  public getAverageTime(): number {
    return this.averageTime;
  }

  public getMinimumOrderValue(): number {
    return this.minimumOrderValue;
  }

  public getMinimumOrderValueText(): string {
    return this.minimumOrderValue.toLocaleString('de', {minimumFractionDigits: 2});
  }

  public getMaximumOrderValue(): number {
    return this.maximumOrderValue;
  }

  public getMaximumOrderValueText(): string {
    return this.maximumOrderValue.toLocaleString('de', {minimumFractionDigits: 2});
  }

  public getCosts(): number {
    return this.costs;
  }

  public getCostsText(): string {
    const val = this.costs;
    if (val === undefined) {
      return undefined;
    } else if (val > 0) {
      return val.toLocaleString('de', {minimumFractionDigits: 2});
    } else {
      return 'Gratis';
    }
  }

  public getHygienicHandling(): string {
    return this.hygienicHandling;
  }

  public hasOrders(): boolean {
    return this.orderedDishes ? this.orderedDishes.length > 0 : false;
  }

  public getOrderedDishes(): OrderedDishModel[] {
    return this.orderedDishes;
  }

  public getDishCountOfOrder(): number {
    let result = 0;
    for (const orderedDish of this.orderedDishes) {
      result += orderedDish.getCount();
    }
    return result;
  }

  public getValueOfOrder(): number {
    if (this.orderedDishes === undefined || this.orderedDishes.length === 0) {
      return 0;
    }
    let result = 0;
    for (const orderedDish of this.orderedDishes) {
      result += orderedDish.getCount() * orderedDish.getVariant().price;
    }
    return result;
  }

  public getValueOfOrderText(): string {
    const val = this.getValueOfOrder();
    return val.toLocaleString('de', {minimumFractionDigits: 2});
  }

  public getMissingValueToMinimum(): number {
    if (this.minimumOrderValue === undefined) {
      return 0;
    }

    let val = this.minimumOrderValue - this.getValueOfOrder();
    if (val < 0) {
      val = 0;
    }

    return val;
  }

  public getMissingValueToMinimumText(): string {
    const val = this.getMissingValueToMinimum();
    return val.toLocaleString('de', {minimumFractionDigits: 2});
  }

  public getTotalPrice(): number {
    if (this.costs !== undefined) {
      return this.getValueOfOrder() + this.costs;
    } else {
      return this.getValueOfOrder();
    }
  }

  public getTotalPriceText(): string {
    const val = this.getTotalPrice();
    return val.toLocaleString('de', {minimumFractionDigits: 2});
  }

  public isVisible(): boolean {
    return this.visible;
  }
}

export enum OrderType {
  Pickup,
  Delivery,
  Reservation
}
