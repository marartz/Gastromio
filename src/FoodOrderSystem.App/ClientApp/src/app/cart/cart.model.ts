import {OrderedDishModel} from './ordered-dish.model';
import {Guid} from 'guid-typescript';
import {DishModel} from '../dish-category/dish.model';
import {DishVariantModel} from '../dish-category/dish-variant.model';

export class CartModel {
  private readonly orderId: string;
  private readonly orderedDishes: OrderedDishModel[];
  private visible: boolean;

  constructor(
    private orderType: OrderType,
    private restaurantId: string,
    private averageTime: number,
    private minimumOrderValue: number,
    private maximumOrderValue: number,
    private costs: number,
    private hygienicHandling: string
  ) {
    this.orderId = Guid.create().toString();
    this.orderedDishes = new Array<OrderedDishModel>();
  }

  public getOrderId(): string {
    return this.orderId;
  }

  public getOrderType(): OrderType {
    return this.orderType;
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

  public changeOrderType(orderType: OrderType, averageTime: number, minimumOrderValue: number,
                         maximumOrderValue: number, costs: number) {
    this.orderType = orderType;
    this.averageTime = averageTime;
    this.minimumOrderValue = minimumOrderValue;
    this.maximumOrderValue = maximumOrderValue;
    this.costs = costs;
  }

  public addOrderedDish(dish: DishModel, variant: DishVariantModel, count: number): void {
    let orderedDish = this.orderedDishes.find(en => en.dish.id === dish.id && en.variant.variantId === variant.variantId);
    if (orderedDish !== undefined) {
      orderedDish.count += count;
    } else {
      orderedDish = new OrderedDishModel();
      orderedDish.dish = dish;
      orderedDish.variant = variant;
      orderedDish.count = count;
      this.orderedDishes.push(orderedDish);
    }
    this.visible = true;
  }

  public incrementCountOfOrderedDish(itemId: string): void {
    const index = this.orderedDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.orderedDishes[index].count++;
  }

  public decrementCountOfOrderedDish(itemId: string): void {
    const index = this.orderedDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.orderedDishes[index].count--;
    if (this.orderedDishes[index].count === 0) {
      this.orderedDishes.splice(index, 1);
    }
  }

  public removeOrderedDish(itemId: string): void {
    const index = this.orderedDishes.findIndex(en => en.itemId === itemId);
    if (index < 0) {
      return;
    }
    this.orderedDishes.splice(index, 1);
  }

  public show(): void {
    this.visible = true;
  }

  public hide(): void {
    this.visible = false;
  }

}

export enum OrderType {
  Pickup,
  Delivery,
  Reservation
}
