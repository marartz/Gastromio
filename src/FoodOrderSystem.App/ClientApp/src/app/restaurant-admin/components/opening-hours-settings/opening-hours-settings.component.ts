import { Component, OnInit } from '@angular/core';

import {Observable} from "rxjs";

import {RestaurantAdminFacade} from "../../restaurant-admin.facade";
import {map} from "rxjs/operators";

@Component({
  selector: 'app-opening-hours-settings',
  templateUrl: './opening-hours-settings.component.html',
  styleUrls: ['./opening-hours-settings.component.css']
})
export class OpeningHoursSettingsComponent implements OnInit {

  openingPeriodRows$: Observable<Array<OpeningPeriodRowViewModel>>

  constructor(
    private facade: RestaurantAdminFacade
  ) { }

  ngOnInit(): void {
    this.openingPeriodRows$ = this.getOpeningPeriodRows$();
  }

  public enableOpeningPeriodSlot(dayOfWeek: number, slotNo: number) {
    this.facade.enableOpeningDaySlot(dayOfWeek, slotNo);
  }

  public disableOpeningPeriodSlot(dayOfWeek: number, slotNo: number) {
    this.facade.disableOpeningDaySlot(dayOfWeek, slotNo);
  }

  private getOpeningPeriodRows$(): Observable<Array<OpeningPeriodRowViewModel>> {
    return this.facade.getRestaurant$()
      .pipe(
        map(restaurant => {
          const tempOpeningSlots = RestaurantAdminFacade.getOpeningSlots(restaurant);

          const result = new Array<OpeningPeriodRowViewModel>(96);

          let curTime = RestaurantAdminFacade.earliestOpeningTime;
          for (let slotNo = 0; slotNo < 96; slotNo++) {
            result[slotNo] = new OpeningPeriodRowViewModel();
            result[slotNo].time = OpeningHoursSettingsComponent.timeToString(curTime);
            result[slotNo].slotNo = slotNo;
            curTime += 15;
          }

          for (let dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++) {
            for (let slotNo = 0; slotNo < 96; slotNo++) {
              result[slotNo].dayOfWeekSlots[dayOfWeek] = new OpeningPeriodSlotViewModel();
              result[slotNo].dayOfWeekSlots[dayOfWeek].dayOfWeek = dayOfWeek;
              result[slotNo].dayOfWeekSlots[dayOfWeek].status = tempOpeningSlots[dayOfWeek][slotNo] !== undefined;
            }
          }

          return result;
        })
      );
  }

  private static timeToString(time: number): string {
    if (time > 24 * 60) {
      time -= 24 * 60;
    }
    const hours = Math.floor(time / 60);
    const minutes = time % 60;
    return OpeningHoursSettingsComponent.padNumber(hours, 2) + ":" + OpeningHoursSettingsComponent.padNumber(minutes, 2);
  }

  private static padNumber(num: number, size: number): string {
    let numText: string = num.toString();
    while (numText.length < size) numText = "0" + numText;
    return numText;
  }

}

export class OpeningPeriodRowViewModel {

  constructor() {
    this.dayOfWeekSlots = new Array<OpeningPeriodSlotViewModel>(7);
  }

  time: string;
  slotNo: number;
  dayOfWeekSlots: Array<OpeningPeriodSlotViewModel>;

}

export class OpeningPeriodSlotViewModel {

  dayOfWeek: number;
  status: boolean;

}
