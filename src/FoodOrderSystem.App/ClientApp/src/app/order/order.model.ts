import {CustomerInfoModel} from './customer-info.model';
import {CartInfoModel} from './cart-info.model';
import {PaymentMethodModel} from '../payment-method/payment-method.model';

export class OrderModel {
  public id: string;
  public customerInfo: CustomerInfoModel;
  public cartInfo: CartInfoModel;
  public comments: string;
  public paymentMethodId: string;
  public paymentMethod: PaymentMethodModel;
  public valueOfOrder: number;
  public costs: number;
  public totalPrice: number;
}

