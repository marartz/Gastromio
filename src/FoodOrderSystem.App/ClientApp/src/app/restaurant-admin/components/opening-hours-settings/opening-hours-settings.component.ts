import {Component, OnDestroy, OnInit} from '@angular/core';

import {BehaviorSubject, Subscription} from "rxjs";

import {RestaurantAdminFacade} from "../../restaurant-admin.facade";
import {OpeningPeriodModel} from "../../../shared/models/restaurant.model";
import {debounceTime} from "rxjs/operators";

@Component({
  selector: 'app-opening-hours-settings',
  templateUrl: './opening-hours-settings.component.html',
  styleUrls: [
    './opening-hours-settings.component.css',
    '../../../../assets/css/frontend_v2.min.css',
    '../../../../assets/css/backend_v2.min.css'
  ]
})
export class OpeningHoursSettingsComponent implements OnInit, OnDestroy {

  public openingHours$: BehaviorSubject<OpeningHoursViewModel>;

  private subscriptions: Array<Subscription>;

  constructor(
    private facade: RestaurantAdminFacade
  ) {
    const openingHours = OpeningHoursSettingsComponent.createOpeningHoursViewModel();
    this.openingHours$ = new BehaviorSubject<OpeningHoursViewModel>(openingHours);
  }

  public ngOnInit(): void {
    this.facade.getRestaurant$()
      .subscribe(restaurant => {
        if (restaurant === undefined || restaurant.openingHours === undefined)
          return;

        this.subscriptions = new Array<Subscription>();

        const openingHours = OpeningHoursSettingsComponent.createOpeningHoursViewModel();
        for (let openingPeriod of restaurant.openingHours) {
          let column = openingHours.weekDays[openingPeriod.dayOfWeek].openingPeriods.length

          const openingPeriodViewModel = new OpeningPeriodViewModel();
          openingPeriodViewModel.column = column;
          openingPeriodViewModel.baseModel = openingPeriod;
          openingPeriodViewModel.dayOfWeek = openingPeriod.dayOfWeek;
          openingPeriodViewModel.start = OpeningHoursSettingsComponent.totalMinutesToString(openingPeriod.start);
          openingPeriodViewModel.end = OpeningHoursSettingsComponent.totalMinutesToString(openingPeriod.end);

          const openingPeriodViewModel$ = new BehaviorSubject<OpeningPeriodViewModel>(openingPeriodViewModel);
          this.addSubscription(openingPeriodViewModel$);

          openingHours.weekDays[openingPeriod.dayOfWeek].openingPeriods.push(openingPeriodViewModel$);
        }

        for (let weekDayOpeningPeriods of openingHours.weekDays) {
          weekDayOpeningPeriods.openingPeriods.sort((a, b) => {
            if (a.value.start < b.value.start)
              return -1;
            else if (a.value.start > b.value.start)
              return +1;
            else
              return 0;
          })
        }

        let maxOpeningPeriodCount = 0;
        for (let i = 0; i < openingHours.weekDays.length; i++) {
          if (openingHours.weekDays[i].openingPeriods.length > maxOpeningPeriodCount) {
            maxOpeningPeriodCount = openingHours.weekDays[i].openingPeriods.length;
          }
        }

        for (let i = 0; i < openingHours.weekDays.length; i++) {
          while (openingHours.weekDays[i].openingPeriods.length < maxOpeningPeriodCount) {
            const openingPeriod$ = new BehaviorSubject<OpeningPeriodViewModel>(undefined);
            this.addSubscription(openingPeriod$);
            openingHours.weekDays[i].openingPeriods.push(openingPeriod$);
          }
        }

        openingHours.columns = new Array<number>();
        for (let i = 0; i < maxOpeningPeriodCount; i++) {
          openingHours.columns.push(i);
        }

        this.openingHours$.next(openingHours);
      });
  }

  public ngOnDestroy(): void {
    this.removeSubscriptions();
  }

  public add(dayOfWeek: number): void {
    const openingHours = this.openingHours$.value;

    let index = openingHours.weekDays[dayOfWeek].openingPeriods.findIndex(en => en !== undefined && en.value !== undefined && en.value.baseModel === undefined);
    if (index >= 0)
      return;

    index = openingHours.weekDays[dayOfWeek].openingPeriods.findIndex(en => en !== undefined && en.value === undefined);

    const openingPeriodViewModel = new OpeningPeriodViewModel();
    openingPeriodViewModel.baseModel = undefined;
    openingPeriodViewModel.dayOfWeek = dayOfWeek;
    openingPeriodViewModel.start = undefined;
    openingPeriodViewModel.end = undefined;

    if (index >= 0) {
      openingPeriodViewModel.column = index;
      openingHours.weekDays[dayOfWeek].openingPeriods[index].next(openingPeriodViewModel);
    } else {
      index = openingHours.columns.length;
      openingPeriodViewModel.column = index;
      for (let weekDay of openingHours.weekDays) {
        const openingPeriod$ = new BehaviorSubject<OpeningPeriodViewModel>(undefined);
        this.addSubscription(openingPeriod$);
        weekDay.openingPeriods.push(openingPeriod$);
      }
      openingHours.columns.push(index);
      openingHours.weekDays[dayOfWeek].openingPeriods[index].next(openingPeriodViewModel);
    }

    this.openingHours$.next(openingHours);
  }

  public changeStart(openingPeriod$: BehaviorSubject<OpeningPeriodViewModel>, newValue: string): void {
    openingPeriod$.value.start = newValue;
    openingPeriod$.next(openingPeriod$.value);
  }

  public changeEnd(openingPeriod$: BehaviorSubject<OpeningPeriodViewModel>, newValue: string): void {
    openingPeriod$.value.end = newValue;
    openingPeriod$.next(openingPeriod$.value);
  }

  public remove(dayOfWeek: number, column: number): void {
    const openingHours = this.openingHours$.value;
    const openingPeriod$ = openingHours.weekDays[dayOfWeek].openingPeriods[column];
    if (openingPeriod$.value === undefined)
      return;

    if (openingPeriod$.value.baseModel === undefined) {
      openingHours.weekDays[dayOfWeek].openingPeriods[column].next(undefined);

      const allUndefined = openingHours.weekDays.every(en => en.openingPeriods[openingHours.columns.length - 1].value === undefined);
      if (allUndefined) {
        for (let weekDay of openingHours.weekDays) {
          weekDay.openingPeriods.splice(openingHours.columns.length - 1, 1);
        }
        openingHours.columns.splice(openingHours.columns.length - 1, 1);
      }

      this.openingHours$.next(openingHours);
    } else {
      this.facade.removeOpeningPeriod(openingPeriod$.value.baseModel);
    }
  }

  private removeSubscriptions(): void {
    if (this.subscriptions === undefined)
      return;

    for (let subscription of this.subscriptions) {
      subscription.unsubscribe();
    }

    this.subscriptions = new Array<Subscription>();
  }

  private addSubscription(openingPeriod$: BehaviorSubject<OpeningPeriodViewModel>): void {
    const subscription = openingPeriod$
      .pipe(
        debounceTime(1000)
      )
      .subscribe(openingPeriod => {
        if (openingPeriod === undefined)
          return;

        const startParseResult = OpeningHoursSettingsComponent.parseTimeValue(openingPeriod.start);
        const endParseResult = OpeningHoursSettingsComponent.parseTimeValue(openingPeriod.end);

        if (!startParseResult.isValid || !endParseResult.isValid) {
          return;
        }

        if (openingPeriod.baseModel === undefined) {
          this.facade.addOpeningPeriod(openingPeriod.dayOfWeek, startParseResult.value, endParseResult.value);
        } else if (openingPeriod.baseModel.start !== startParseResult.value || openingPeriod.baseModel.end !== endParseResult.value) {
          this.facade.changeOpeningPeriod(openingPeriod.baseModel, startParseResult.value, endParseResult.value);
        }
      });

    this.subscriptions.push(subscription);
  }

  private static createOpeningHoursViewModel(): OpeningHoursViewModel {
    const openingHours = new OpeningHoursViewModel();

    openingHours.weekDays = new Array<WeekDayOpeningPeriodListViewModel>(7);
    openingHours.weekDays[0] = OpeningHoursSettingsComponent.createWeekDayOpeningPeriod(0, "Montag");
    openingHours.weekDays[1] = OpeningHoursSettingsComponent.createWeekDayOpeningPeriod(1, "Dienstag");
    openingHours.weekDays[2] = OpeningHoursSettingsComponent.createWeekDayOpeningPeriod(2, "Mittwoch");
    openingHours.weekDays[3] = OpeningHoursSettingsComponent.createWeekDayOpeningPeriod(3, "Donnerstag");
    openingHours.weekDays[4] = OpeningHoursSettingsComponent.createWeekDayOpeningPeriod(4, "Freitag");
    openingHours.weekDays[5] = OpeningHoursSettingsComponent.createWeekDayOpeningPeriod(5, "Samstag");
    openingHours.weekDays[6] = OpeningHoursSettingsComponent.createWeekDayOpeningPeriod(6, "Sonntag");

    return openingHours;
  }

  private static createWeekDayOpeningPeriod(dayOfWeek: number, dayOfWeekText: string): WeekDayOpeningPeriodListViewModel {
    const weekDayOpeningPeriods = new WeekDayOpeningPeriodListViewModel();
    weekDayOpeningPeriods.dayOfWeek = dayOfWeek;
    weekDayOpeningPeriods.dayOfWeekText = dayOfWeekText;
    weekDayOpeningPeriods.openingPeriods = new Array<BehaviorSubject<OpeningPeriodViewModel>>();
    return weekDayOpeningPeriods;
  }

  private static totalMinutesToString(totalMinutes: number): string {
    const hours = Math.floor(totalMinutes / 60);
    const minutes = Math.floor(totalMinutes % 60);
    return hours.toString().padStart(2, '0') + ':' + minutes.toString().padStart(2, '0');
  }

  private static parseTimeValue(text: string): TimeParseResult {
    if (!text) {
      return new TimeParseResult(false, 0);
    }

    text = text.trim();

    if (text.length < 5) {
      return new TimeParseResult(false, 0);
    }

    text = text.substr(0, 5);

    if (text.substr(2, 1) !== ':') {
      return new TimeParseResult(false, 0);
    }

    const hoursText = text.substr(0, 2);
    const hours = Number(hoursText);
    if (hours === Number.NaN) {
      return new TimeParseResult(false, 0);
    }
    if (hours < 0 || hours > 23) {
      return new TimeParseResult(false, 0);
    }

    const minutesText = text.substr(3, 2);
    const minutes = Number(minutesText);
    if (minutes === Number.NaN) {
      return new TimeParseResult(false, 0);
    }
    if (minutes < 0 || minutes > 59) {
      return new TimeParseResult(false, 0);
    }

    return new TimeParseResult(true, hours * 60 + minutes);
  }

//   onAddOpeningPeriod(value): void {
//     const dayOfWeek: number = Number(value.dayOfWeek);
//     const invalidNumberError: string = 'Geben Sie eine gültige Zeit ein';
//
//     const startParseResult: TimeParseResult = AdminRestaurantComponent.parseTimeValue(value.start);
//     if (!startParseResult.isValid) {
//       this.formError = invalidNumberError;
//       return;
//     } else {
//       this.formError = undefined;
//     }
//
//     const endParseResult: TimeParseResult = AdminRestaurantComponent.parseTimeValue(value.end);
//     if (!endParseResult.isValid) {
//       this.formError = invalidNumberError;
//       return;
//     } else {
//       this.formError = undefined;
//     }
//
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.addOpeningPeriodToRestaurantAsync(this.restaurant.id, dayOfWeek,
//       startParseResult.value, endParseResult.value)
//       .pipe(take(1))
//       .subscribe(() => {
//         this.blockUI.stop();
//
//         this.formError = undefined;
//         this.addOpeningPeriodForm.reset();
//
//         const model = new OpeningPeriodModel();
//         model.dayOfWeek = dayOfWeek;
//         model.start = startParseResult.value;
//         model.end = endParseResult.value;
//
//         this.restaurant.openingHours.push(model);
//         this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//
//   onRemoveOpeningPeriod(openingPeriod: OpeningPeriodViewModel) {
//     this.blockUI.start('Verarbeite Daten...');
//     this.restaurantRestAdminService.removeOpeningPeriodFromRestaurantAsync(this.restaurant.id, openingPeriod.dayOfWeek,
//       openingPeriod.startTime)
//       .pipe(take(1))
//       .subscribe(() => {
//         this.blockUI.stop();
//         this.formError = undefined;
//
//         const index = this.restaurant.openingHours.findIndex(elem => elem.dayOfWeek === openingPeriod.dayOfWeek
//           && elem.start === openingPeriod.startTime);
//         if (index > -1) {
//           this.restaurant.openingHours.splice(index, 1);
//           this.openingPeriodVMs = OpeningPeriodViewModel.vmArrayFromModels(this.restaurant.openingHours);
//         }
//       }, (response: HttpErrorResponse) => {
//         this.blockUI.stop();
//         this.formError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
//       });
//   }
//

}

export class OpeningHoursViewModel {

  public columns: Array<number>;
  public weekDays: Array<WeekDayOpeningPeriodListViewModel>;

}

export class WeekDayOpeningPeriodListViewModel {

  public dayOfWeek: number;
  public dayOfWeekText: string;
  public openingPeriods: Array<BehaviorSubject<OpeningPeriodViewModel>>;

}

export class OpeningPeriodViewModel {

  public column: number;
  public baseModel: OpeningPeriodModel;
  public dayOfWeek: number;
  public start: string;
  public end: string;

}

class TimeParseResult {

  constructor(isValid: boolean, value: number) {
    this.isValid = isValid;
    this.value = value;
  }

  isValid: boolean;
  value: number;

}
