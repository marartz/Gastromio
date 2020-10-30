import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal, NgbCalendar, NgbDateStruct, NgbTimeStruct} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-opening-hour-filter',
  templateUrl: './opening-hour-filter.component.html',
  styleUrls: [
    './opening-hour-filter.component.css',
    '../../assets/css/frontend_v3.min.css',
  ]
})
export class OpeningHourFilterComponent implements OnInit {

  @Input() public value: Date;

  minDate: NgbDateStruct;
  date: NgbDateStruct;
  time: NgbTimeStruct;

  constructor(
    public activeModal: NgbActiveModal,
    private calendar: NgbCalendar
  ) {
  }

  ngOnInit() {
    const now = OpeningHourFilterComponent.roundOnQuarterHours(new Date());

    this.minDate = {year: now.getFullYear(), month: now.getMonth() + 1, day: now.getDate()};

    if (this.value !== undefined) {
      if (this.value < now) {
        this.value = now;
      }
      this.date = {year: this.value.getFullYear(), month: this.value.getMonth() + 1, day: this.value.getDate()};
      this.time = {hour: this.value.getHours(), minute: this.value.getMinutes(), second: 0};
    } else {
      this.date = this.calendar.getToday();
      this.time = {hour: now.getHours(), minute: now.getMinutes(), second: 0};
    }
  }

  onTimeChanged(): void {
    const now = OpeningHourFilterComponent.roundOnQuarterHours(new Date());

    let date = this.calculateDate();

    if (date < now) {
      this.time = {hour: now.getHours(), minute: now.getMinutes(), second: 0};
    }
  }

  onClose(): void {
    const now = OpeningHourFilterComponent.roundOnQuarterHours(new Date());

    let date = this.calculateDate();

    if (date < now) {
      date = now;
    }

    this.activeModal.close(date);
  }

  private static roundOnQuarterHours(date: Date): Date {
    let minutesToAdd = Math.ceil(date.getMinutes() / 15) * 15 - date.getMinutes();
    return new Date(date.getTime() + minutesToAdd * 60000);
  }

  private calculateDate(): Date {
    return new Date(
      this.date.year,
      this.date.month - 1,
      this.date.day,
      this.time.hour,
      this.time.minute,
      0,
      0
    );
  }

}
