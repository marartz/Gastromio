import { PaymentMethodModel } from '../../shared/models/payment-method.model';

import { CustomerInfoModel } from './customer-info.model';
import { CartInfoModel } from './cart-info.model';

export class OrderModel {
  constructor(init?: Partial<OrderModel>) {
    if (init) {
      Object.assign(this, init);
    }
    this.customerInfo = this.customerInfo
      ? new CustomerInfoModel(this.customerInfo)
      : undefined;
    this.cartInfo = this.cartInfo
      ? new CartInfoModel(this.cartInfo)
      : undefined;
    this.paymentMethod = this.paymentMethod
      ? new PaymentMethodModel(this.paymentMethod)
      : undefined;
  }

  public id: string;

  public customerInfo: CustomerInfoModel;

  public cartInfo: CartInfoModel;

  public comments: string;

  public paymentMethodId: string;

  public paymentMethod: PaymentMethodModel;

  public valueOfOrder: number;

  public costs: number;

  public totalPrice: number;

  public clone(): OrderModel {
    return new OrderModel({
      id: this.id,
      customerInfo: this.customerInfo?.clone(),
      cartInfo: this.cartInfo?.clone(),
      comments: this.comments,
      paymentMethodId: this.paymentMethodId,
      paymentMethod: this.paymentMethod?.clone(),
      valueOfOrder: this.valueOfOrder,
      costs: this.costs,
      totalPrice: this.totalPrice,
    });
  }
}
