import {PaymentMethodModel} from '../payment-method/payment-method.model';
import {UserModel} from '../user/user.model';
import {CuisineModel} from '../cuisine/cuisine.model';

export class RestaurantModel {

  constructor(init?: Partial<RestaurantModel>) {
    if (init) {
      Object.assign(this, init);
    }

    if (!this.address) {
      this.address = new AddressModel();
    } else {
      this.address = new AddressModel(init.address);
    }

    if (this.contactInfo) {
      this.contactInfo = new ContactInfoModel(this.contactInfo);
    }

    if (!this.openingHours) {
      this.openingHours = new Array<OpeningPeriodModel>();
    } else {
      for (let i = 0; i < this.openingHours.length; i++) {
        this.openingHours[i] = new OpeningPeriodModel(this.openingHours[i]);
      }
    }

    if (this.pickupInfo) {
      this.pickupInfo = new PickupInfoModel(this.pickupInfo);
    }

    if (this.deliveryInfo) {
      this.deliveryInfo = new DeliveryInfoModel(this.deliveryInfo);
    }

    if (this.reservationInfo) {
      this.reservationInfo = new ReservationInfoModel(this.reservationInfo);
    }
  }


  public id: string;

  public name: string;

  public address: AddressModel;

  public contactInfo: ContactInfoModel;

  public imageTypes: string[];

  public openingHours: OpeningPeriodModel[];

  public openingHoursText: string;

  public openingHoursTodayText: string;

  public pickupInfo: PickupInfoModel;

  public deliveryInfo: DeliveryInfoModel;

  public reservationInfo: ReservationInfoModel;

  public hygienicHandling: string;

  public cuisines: CuisineModel[];

  public cuisinesText: string;

  public paymentMethods: PaymentMethodModel[];

  public administrators: UserModel[];

  public isActive: boolean;

  public needsSupport: boolean;

  isOpen(date: Date): boolean {
    if (!this.openingHours) {
      return false;
    }

    try {
      let dayOfWeek = (date.getDay() - 1) % 7; // DayOfWeek starts with Sunday
      if (dayOfWeek < 0) {
        dayOfWeek += 7;
      }
      let time = date.getHours() * 60 + date.getMinutes();
      if (date.getHours() < 4) {
        dayOfWeek = (dayOfWeek - 1) % 7;
        if (dayOfWeek < 0) {
          dayOfWeek += 7;
        }
        time += 24 * 60;
      }

      let isOpen: boolean;
      isOpen = this.openingHours.some(en => en.dayOfWeek === dayOfWeek && en.start <= time && time <= en.end);

      return isOpen;
    } catch (e) {
      console.error(e);
      return false;
    }
  }
}

export class AddressModel {
  constructor(init?: Partial<AddressModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public street: string;

  public zipCode: string;

  public city: string;
}

export class ContactInfoModel {
  constructor(init?: Partial<ContactInfoModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public phone: string;

  public fax: string;

  public webSite: string;

  public responsiblePerson: string;

  public emailAddress: string;
}

export class OpeningPeriodModel {
  constructor(init?: Partial<OpeningPeriodModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public dayOfWeek: number;

  public start: number;

  public end: number;
}

export class PickupInfoModel {
  constructor(init?: Partial<PickupInfoModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public enabled: boolean;

  public averageTime: number;

  public minimumOrderValue: number;

  public minimumOrderValueText: string;

  public maximumOrderValue: number;

  public maximumOrderValueText: string;
}

export class DeliveryInfoModel {
  constructor(init?: Partial<DeliveryInfoModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public enabled: boolean;

  public averageTime: number;

  public minimumOrderValue: number;

  public minimumOrderValueText: string;

  public maximumOrderValue: number;

  public maximumOrderValueText: string;

  public costs: number;

  public getCostsText(): string {
    const val = this.costs;
    if (val === undefined) {
      return undefined;
    } else if (val > 0) {
      return 'Min. ' + val.toLocaleString('de', {minimumFractionDigits: 2}) + ' €';
    } else {
      return 'Gratis';
    }
  }
}

export class ReservationInfoModel {
  constructor(init?: Partial<ReservationInfoModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

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
