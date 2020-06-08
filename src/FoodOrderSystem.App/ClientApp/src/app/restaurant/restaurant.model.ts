import { PaymentMethodModel } from '../payment-method/payment-method.model';
import { UserModel } from '../user/user.model';
import { CuisineModel } from '../cuisine/cuisine.model';

export class RestaurantModel {
  constructor() {
    this.address = new AddressModel();
    this.openingHours = new Array<OpeningPeriodModel>();
  }

  public id: string;

  public image: string;

  public name: string;

  public address: AddressModel;

  public contactInfo: ContactInfoModel;

  public openingHours: OpeningPeriodModel[];

  public pickupInfo: PickupInfoModel;

  public deliveryInfo: DeliveryInfoModel;

  public reservationInfo: ReservationInfoModel;

  public cuisines: CuisineModel[];

  public paymentMethods: PaymentMethodModel[];

  public administrators: UserModel[];
}

export class AddressModel {
  constructor() { }

  public street: string;

  public zipCode: string;

  public city: string;
}

export class ContactInfoModel {
  constructor() {  }

  public phone: string;

  public fax: string;

  public webSite: string;

  public responsiblePerson: string;

  public emailAddress: string;
}

export class OpeningPeriodModel {
  constructor() { }

  public dayOfWeek: number;

  public start: number;

  public end: number;
}

export class PickupInfoModel {
  public averageTime: number;

  public minimumOrderValue: number;

  public minimumOrderValueText: string;

  public maximumOrderValue: number;

  public maximumOrderValueText: string;

  public hygienicHandling: string;
}

export class DeliveryInfoModel {
  public averageTime: number;

  public minimumOrderValue: number;

  public minimumOrderValueText: string;

  public maximumOrderValue: number;

  public maximumOrderValueText: string;

  public costs: number;

  public costsText: string;

  public hygienicHandling: string;
}

export class ReservationInfoModel {
  public hygienicHandling: string;
}
