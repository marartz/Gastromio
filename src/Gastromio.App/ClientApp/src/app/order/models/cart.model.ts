import { CartDishModel } from './cart-dish.model';
import { OrderType } from './order-type';

export class CartModel {
  constructor(
    private orderType: OrderType,
    private restaurantId: string,
    private averageTime: number,
    private minimumOrderValue: number,
    private maximumOrderValue: number,
    private costs: number,
    private hygienicHandling: string,
    private cartDishes: CartDishModel[],
    private visible: boolean,
    private serviceTime: Date
  ) {}

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

  public isPickup(): boolean {
    return this.orderType === OrderType.Pickup;
  }

  public isDelivery(): boolean {
    return this.orderType === OrderType.Delivery;
  }

  public isReservation(): boolean {
    return this.orderType === OrderType.Reservation;
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
    if (!this.minimumOrderValue) return '0';
    return this.minimumOrderValue.toLocaleString('de', {
      minimumFractionDigits: 2,
    });
  }

  public getMaximumOrderValue(): number {
    return this.maximumOrderValue;
  }

  public getMaximumOrderValueText(): string {
    if (!this.maximumOrderValue) return '0';
    return this.maximumOrderValue.toLocaleString('de', {
      minimumFractionDigits: 2,
    });
  }

  public getCosts(): number {
    return this.costs;
  }

  public getCostsText(): string {
    const val = this.costs;
    if (val === undefined) {
      return undefined;
    } else if (val > 0) {
      return val.toLocaleString('de', { minimumFractionDigits: 2 });
    } else {
      return 'Gratis';
    }
  }

  public getHygienicHandling(): string {
    return this.hygienicHandling;
  }

  public hasOrders(): boolean {
    return this.cartDishes ? this.cartDishes.length > 0 : false;
  }

  public getCartDishes(): CartDishModel[] {
    return this.cartDishes;
  }

  public getDishCountOfOrder(): number {
    let result = 0;
    for (const cartDish of this.cartDishes) {
      result += cartDish.getCount();
    }
    return result;
  }

  public getValueOfOrder(): number {
    if (this.cartDishes === undefined || this.cartDishes.length === 0) {
      return 0;
    }
    let result = 0;
    for (const cartDish of this.cartDishes) {
      result += cartDish.getCount() * cartDish.getVariant().price;
    }
    return result;
  }

  public getValueOfOrderText(): string {
    const val = this.getValueOfOrder();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
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
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
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
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public isVisible(): boolean {
    return this.visible;
  }

  public getServiceTime(): Date {
    return this.serviceTime;
  }

  public isValid(): boolean {
    return !this.getValidationError();
  }

  public getValidationError(): string {
    const valueOfOrder = this.getValueOfOrder();

    if (this.minimumOrderValue && valueOfOrder < this.minimumOrderValue) {
      return (
        'Der Mindestbestellwert von € ' +
        this.getMinimumOrderValueText() +
        ' ist nicht erreicht.'
      );
    }

    if (this.maximumOrderValue && valueOfOrder > this.maximumOrderValue) {
      return (
        'Der Maximalbestellwert von € ' +
        this.getMaximumOrderValueText() +
        ' ist überschritten'
      );
    }

    return undefined;
  }

  public clone(): CartModel {
    return new CartModel(
      this.orderType,
      this.restaurantId,
      this.averageTime,
      this.minimumOrderValue,
      this.maximumOrderValue,
      this.costs,
      this.hygienicHandling,
      this.cartDishes?.map((dish) => dish?.clone()),
      this.visible,
      this.serviceTime
    );
  }
}
