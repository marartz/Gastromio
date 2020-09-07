import {Component, OnInit, Input} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

import {UserAdminService} from '../user/user-admin.service';
import {UserModel} from '../user/user.model';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {HttpErrorResponse} from '@angular/common/http';
import {ConfirmPasswordValidator} from '../validators/password.validator';
import {take} from "rxjs/operators";

@Component({
  selector: 'app-change-user-password',
  templateUrl: './change-user-password.component.html',
  styleUrls: [
    './change-user-password.component.css',
    '../../assets/css/frontend_v2.min.css',
    '../../assets/css/backend_v2.min.css',
    '../../assets/css/animations_v2.min.css'
  ]
})
export class ChangeUserPasswordComponent implements OnInit {
  @Input() public user: UserModel;
  @BlockUI() blockUI: NgBlockUI;

  changeUserPasswordForm: FormGroup;
  message: string;
  submitted = false;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private userAdminService: UserAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
    this.changeUserPasswordForm = this.formBuilder.group({
      password: ['', [Validators.required, Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&]).{6,}')]],
      passwordRepeat: ['']
    }, {validators: ConfirmPasswordValidator('password', 'passwordRepeat')});
  }

  get f() {
    return this.changeUserPasswordForm.controls;
  }

  onSubmit(data) {
    this.submitted = true;
    if (this.changeUserPasswordForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.userAdminService.changeUserPasswordAsync(this.user.id, data.password)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.changeUserPasswordForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}