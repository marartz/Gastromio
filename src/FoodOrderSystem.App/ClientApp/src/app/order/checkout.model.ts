export class CheckoutModel {
  public givenName: string;
  public lastName: string;
  public street: string;
  public addAddressInfo: string;
  public zipCode: string;
  public city: string;
  public phone: string;
  public email: string;
  public orderType: string;
  public restaurantId: string;
  public orderedDishes: OrderedDishInfoModel[];
  public comments: string;
}

export class OrderedDishInfoModel {
  public itemId: string;
  public dishId: string;
  public variantId: string;
  public count: number;
  public remarks: string;
}
