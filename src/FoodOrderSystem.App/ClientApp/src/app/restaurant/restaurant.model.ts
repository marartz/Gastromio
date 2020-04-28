export class RestaurantModel {
  constructor() {
    this.address = new AddressModel();
    this.deliveryTimes = new Array<DeliveryTimeModel>();
  }

  public id: string;

  public name: string;

  public address: AddressModel;

  public deliveryTimes: DeliveryTimeModel[];

  public minimumOrderValue: number;

  public deliveryCosts: number;

  public webSite: string;

  public imprint: string;
}

export class AddressModel {
  constructor() { }

  public line1: string;

  public line2: string;

  public zipCode: string;

  public city: string;
}

export class DeliveryTimeModel {
  constructor() { }

  public dayOfWeek: number;

  public start: number;

  public end: number;
}
