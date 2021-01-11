import {Component, OnInit, Input} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {CuisineModel} from '../../../shared/models/cuisine.model';

import {HttpErrorHandlingService} from '../../../shared/services/http-error-handling.service';

import {CuisineAdminService} from '../../services/cuisine-admin.service';

@Component({
  selector: 'app-change-cuisine',
  templateUrl: './change-cuisine.component.html',
  styleUrls: [
    './change-cuisine.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class ChangeCuisineComponent implements OnInit {
  @Input() public cuisine: CuisineModel;
  @BlockUI() blockUI: NgBlockUI;

  changeCuisineForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private cuisineAdminService: CuisineAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
    this.changeCuisineForm = this.formBuilder.group({
      name: [this.cuisine.name, Validators.required]
    });
  }

  get f() {
    return this.changeCuisineForm.controls;
  }

  onSubmit(data) {
    if (this.changeCuisineForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.cuisineAdminService.changeCuisineAsync(this.cuisine.id, data.name)
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.changeCuisineForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
