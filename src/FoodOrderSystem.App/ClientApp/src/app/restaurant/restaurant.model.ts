import {PaymentMethodModel} from '../payment-method/payment-method.model';
import {UserModel} from '../user/user.model';
import {CuisineModel} from '../cuisine/cuisine.model';

export class RestaurantModel {
  constructor() {
    this.address = new AddressModel();
    this.openingHours = new Array<OpeningPeriodModel>();
  }

  public id: string;

  public name: string;

  public address: AddressModel;

  public contactInfo: ContactInfoModel;

  public imageTypes: string[];

  public openingHours: OpeningPeriodModel[];

  public openingHoursText: string;

  public pickupInfo: PickupInfoModel;

  public deliveryInfo: DeliveryInfoModel;

  public reservationInfo: ReservationInfoModel;

  public hygienicHandling: string;

  public cuisines: CuisineModel[];

  public paymentMethods: PaymentMethodModel[];

  public administrators: UserModel[];

  public isActive: boolean;
}

export class AddressModel {
  constructor() {
  }

  public street: string;

  public zipCode: string;

  public city: string;
}

export class ContactInfoModel {
  constructor() {
  }

  public phone: string;

  public fax: string;

  public webSite: string;

  public responsiblePerson: string;

  public emailAddress: string;
}

export class OpeningPeriodModel {
  constructor() {
  }

  public dayOfWeek: number;

  public start: number;

  public end: number;
}

export class PickupInfoModel {
  public enabled: boolean;

  public averageTime: number;

  public minimumOrderValue: number;

  public minimumOrderValueText: string;

  public maximumOrderValue: number;

  public maximumOrderValueText: string;
}

export class DeliveryInfoModel {
  public enabled: boolean;

  public averageTime: number;

  public minimumOrderValue: number;

  public minimumOrderValueText: string;

  public maximumOrderValue: number;

  public maximumOrderValueText: string;

  public costs: number;

  public costsText: string;
}

export class ReservationInfoModel {
  public enabled: boolean;
}

export class ServiceInfoModel {
  public pickupEnabled: boolean;

  public pickupAverageTime: number;

  public pickupMinimumOrderValue: number;

  public pickupMaximumOrderValue: number;

  public deliveryEnabled: boolean;

  public deliveryAverageTime: number;

  public deliveryMinimumOrderValue: number;

  public deliveryMaximumOrderValue: number;

  public deliveryCosts: number;

  public reservationEnabled: boolean;

  public hygienicHandling: string;
}
