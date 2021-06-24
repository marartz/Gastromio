import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";

import {NgbActiveModal, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {DateModel} from "../../../shared/models/date.model";
import {ConfirmPasswordValidator} from "../../../auth/validators/password.validator";

@Component({
  selector: 'app-add-deviating-date',
  templateUrl: './add-deviating-date.component.html',
  styleUrls: [
    './add-deviating-date.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css',
    '../../../../assets/css/overlays/modals.min.css'
  ]
})
export class AddDeviatingDateComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;

  minDate: NgbDateStruct;
  date: NgbDateStruct;

  form: FormGroup;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder
  ) {
    const now = new Date();
    this.minDate = {year: now.getFullYear(), month: now.getMonth() + 1, day: now.getDate()};

    this.form = this.formBuilder.group({
      status: ['open']
    });
  }

  ngOnInit(): void {
  }

  onSubmit(): void {
    this.activeModal.close(new DateModel(this.date.year, this.date.month, this.date.day));
  }

}
