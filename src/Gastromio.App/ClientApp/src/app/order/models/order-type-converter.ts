import { OrderType } from './order-type';

export class OrderTypeConverter {
  public static convertFromString(orderType: string): OrderType {
    switch (orderType) {
      case 'pickup':
        return OrderType.Pickup;
      case 'delivery':
        return OrderType.Delivery;
      case 'reservation':
        return OrderType.Reservation;
      default:
        throw new Error('unknown order type: ' + orderType);
    }
  }

  public static convertToString(orderType: OrderType): string {
    switch (orderType) {
      case OrderType.Pickup:
        return 'pickup';
      case OrderType.Delivery:
        return 'delivery';
      case OrderType.Reservation:
        return 'reservation';
    }
  }
}
