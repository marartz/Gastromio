import {Component, OnInit, Input} from '@angular/core';

import {NgbActiveModal, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

@Component({
  selector: 'app-add-deviating-date',
  templateUrl: './add-deviating-date.component.html',
  styleUrls: [
    './add-deviating-date.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class AddDeviatingDateComponent implements OnInit {

  @BlockUI() blockUI: NgBlockUI;

  minDate: NgbDateStruct;
  date: NgbDateStruct;

  constructor(
    public activeModal: NgbActiveModal,
  ) {
    const now = new Date();
    this.minDate = {year: now.getFullYear(), month: now.getMonth() + 1, day: now.getDate()};
  }

  ngOnInit(): void {
  }

  onSubmit() {
    this.activeModal.close(this.date);
  }

}
