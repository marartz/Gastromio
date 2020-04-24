import { PaymentMethodModel } from "../payment-method/payment-method.model";

export class RestaurantModel {
  constructor() { }

  public id: string;

  public name: string;

  public address: RestaurantAddress;

  public deliveryTimes: DeliveryTime[];

  public minimumOrderValue: number;

  public deliveryCosts: number;

  public webSite: string;

  public imprint: string;

  public paymentMethods: PaymentMethodModel[];
}

export class RestaurantAddress {
  constructor() { }

  public line1: string;

  public line2: string;

  public zipCode: string;

  public city: string;
}

export class DeliveryTime {
  constructor() { }

  public dayOfWeek: number;

  public start: number;

  public end: number;
}

