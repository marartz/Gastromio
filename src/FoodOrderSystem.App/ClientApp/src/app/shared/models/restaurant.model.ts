import {PaymentMethodModel} from './payment-method.model';
import {UserModel} from './user.model';
import {CuisineModel} from './cuisine.model';
import {DateModel} from "./date.model";

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

    if (!this.regularOpeningDays) {
      this.regularOpeningDays = new Array<RegularOpeningDayModel>();
    } else {
      for (let i = 0; i < this.regularOpeningDays.length; i++) {
        this.regularOpeningDays[i] = new RegularOpeningDayModel(this.regularOpeningDays[i]);
      }
    }

    if (!this.deviatingOpeningDays) {
      this.deviatingOpeningDays = new Array<DeviatingOpeningDayModel>();
    } else {
      for (let i = 0; i < this.deviatingOpeningDays.length; i++) {
        this.deviatingOpeningDays[i] = new DeviatingOpeningDayModel(this.deviatingOpeningDays[i]);
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

    if (this.externalMenus) {
      for (let i = 0; i < this.externalMenus.length; i++) {
        this.externalMenus[i] = new ExternalMenu(this.externalMenus[i]);
      }
    }

    if (this.paymentMethods) {
      this.paymentMethods.sort((a, b) => {
        if (a.name < b.name)
          return -1
        else if (a.name > b.name)
          return 1;
        return 0;
      })
    }
  }


  public id: string;

  public name: string;

  public alias: string;

  public address: AddressModel;

  public contactInfo: ContactInfoModel;

  public imageTypes: string[];

  public regularOpeningDays: Array<RegularOpeningDayModel>;

  public deviatingOpeningDays: Array<DeviatingOpeningDayModel>;

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

  public supportedOrderMode: string;

  public externalMenus: ExternalMenu[];

  public isOpen(dateTime: Date): boolean {
    if (dateTime === undefined || dateTime < new Date())
      dateTime = new Date();
    return this.findOpeningPeriod(dateTime) !== undefined;
  }

  public getRestaurantClosedReason(dateTime: Date): string {
    if (dateTime === undefined || dateTime < new Date())
      dateTime = new Date();
    const date = new DateModel(dateTime.getFullYear(), dateTime.getMonth() + 1, dateTime.getDate());
    const deviatingOpeningDay = this.deviatingOpeningDays?.find(en => DateModel.isEqual(en.date, date));
    if (!deviatingOpeningDay)
      return "geschlossen";

    if (deviatingOpeningDay.status === "fully-booked") {
      return "ausgebucht";
    } else {
      return "geschlossen";
    }
  }

  public isOrderPossibleAt(orderDateTime: Date): boolean {
    const now = new Date();

    if (orderDateTime === undefined || orderDateTime < now)
      orderDateTime = new Date();

    const today = new Date();
    today.setHours(0, 0, 0, 0);

    const orderDate = new Date(orderDateTime.getFullYear(), orderDateTime.getMonth(), orderDateTime.getDate());

    if (this.supportedOrderMode === 'phone') {
      return false;
    }

    const openingPeriodOfOrderDateTime = this.findOpeningPeriod(orderDateTime);
    if (openingPeriodOfOrderDateTime === undefined) {
      return false;
    }

    if (this.supportedOrderMode !== 'shift') {
      return true;
    }

    if (orderDate > today) {
      return true;
    }

    const openingPeriodOfNow = this.findOpeningPeriod(now);
    if (openingPeriodOfNow === undefined) {
      return true;
    }

    return openingPeriodOfNow !== openingPeriodOfOrderDateTime;
  }

  private findOpeningPeriod(dateTime: Date): OpeningPeriodModel {
    let dayOfWeek = (dateTime.getDay() - 1) % 7; // DayOfWeek starts with Sunday
    if (dayOfWeek < 0) {
      dayOfWeek += 7;
    }
    let time = dateTime.getHours() * 60 + dateTime.getMinutes();
    if (dateTime.getHours() < 4) {
      dayOfWeek = (dayOfWeek - 1) % 7;
      if (dayOfWeek < 0) {
        dayOfWeek += 7;
      }
      time += 24 * 60;
    }

    if (this.deviatingOpeningDays) {
      const date = new DateModel(dateTime.getFullYear(), dateTime.getMonth() + 1, dateTime.getDate());
      const deviatingOpeningDay = this.deviatingOpeningDays.find(en => DateModel.isEqual(en.date, date));
      if (deviatingOpeningDay) {
        return deviatingOpeningDay?.openingPeriods.find(en => en.start <= time && time <= en.end);
      }
    }

    if (this.regularOpeningDays) {
      const regularOpeningDay = this.regularOpeningDays.find(en => en.dayOfWeek === dayOfWeek);
      if (regularOpeningDay) {
        return regularOpeningDay?.openingPeriods.find(en => en.start <= time && time <= en.end);
      }
    }

    return undefined;
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

  start: number;

  end: number;
}

export class RegularOpeningDayModel {
  constructor(init?: Partial<RegularOpeningDayModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public dayOfWeek: number;

  public openingPeriods: Array<OpeningPeriodModel>;
}

export class DeviatingOpeningDayModel {
  constructor(init?: Partial<DeviatingOpeningDayModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public date: DateModel;

  public status: string;

  public openingPeriods: Array<OpeningPeriodModel>;
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
  constructor(init?: Partial<ServiceInfoModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }
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

export class ExternalMenu {
  constructor(init?: Partial<ExternalMenu>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public id: string;

  public name: string;

  public description: string;

  public url: string;
}
