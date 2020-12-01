import {Component, OnInit, Input} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

import {take} from 'rxjs/operators';

import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {UserModel} from '../../../shared/models/user.model';

import {HttpErrorHandlingService} from '../../../shared/services/http-error-handling.service';

import {UserAdminService} from '../../services/user-admin.service';

@Component({
  selector: 'app-change-user-details',
  templateUrl: './change-user-details.component.html',
  styleUrls: [
    './change-user-details.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class ChangeUserDetailsComponent implements OnInit {
  @Input() public user: UserModel;
  @BlockUI() blockUI: NgBlockUI;

  changeUserDetailsForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private userAdminService: UserAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService,
  ) {
  }

  ngOnInit() {
    this.changeUserDetailsForm = this.formBuilder.group({
      role: [this.user.role, Validators.required],
      email: [this.user.email, [Validators.required, Validators.email, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]]
    });
  }

  get f() {
    return this.changeUserDetailsForm.controls;
  }

  onSubmit(data) {
    if (this.changeUserDetailsForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.userAdminService.changeUserDetailsAsync(this.user.id, data.role, data.email)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.changeUserDetailsForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
