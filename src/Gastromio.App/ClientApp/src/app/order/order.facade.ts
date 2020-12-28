import {Injectable} from "@angular/core";

import {BehaviorSubject, Observable} from "rxjs";

import {OrderService} from "./services/order.service";
import {OrderType} from "./models/cart.model";

@Injectable()
export class OrderFacade {

  private selectedOrderType$: BehaviorSubject<OrderType> = new BehaviorSubject<OrderType>(OrderType.Pickup);
  private selectedOpeningHour$: BehaviorSubject<Date> = new BehaviorSubject<Date>(undefined);
  private selectedCuisine$: BehaviorSubject<string> = new BehaviorSubject<string>(undefined);

  constructor(
    private orderService: OrderService
  ) {
  }

  public getSelectedOrderType$(): Observable<OrderType> {
    return this.selectedOrderType$;
  }

  public getSelectedOrderType(): OrderType {
    return this.selectedOrderType$.value;
  }

  public setSelectedOrderType(selectedOrderType: OrderType): void {
    this.selectedOrderType$.next(selectedOrderType);
  }

  public getSelectedOpeningHour$(): Observable<Date> {
    return this.selectedOpeningHour$;
  }

  public getSelectedOpeningHour(): Date {
    return this.selectedOpeningHour$.value;
  }

  public setSelectedOpeningHour(selectedOpeningHourFilter: Date): void {
    this.selectedOpeningHour$.next(selectedOpeningHourFilter);
  }

  public getSelectedCuisine$(): Observable<string> {
    return this.selectedCuisine$;
  }

  public getSelectedCuisine(): string {
    return this.selectedCuisine$.value;
  }

  public setSelectedCuisine(selectedCuisineFilter: string): void {
    this.selectedCuisine$.next(selectedCuisineFilter);
  }

}
