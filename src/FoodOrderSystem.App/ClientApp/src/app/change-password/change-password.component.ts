import {Component, OnInit, Input} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {HttpErrorResponse} from '@angular/common/http';
import {ConfirmPasswordValidator} from '../validators/password.validator';
import {concatMap, take, tap} from "rxjs/operators";
import {AuthService} from "../auth/auth.service";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: [
    './change-password.component.css',
    '../../assets/css/frontend_v2.min.css',
    '../../assets/css/backend_v2.min.css',
    '../../assets/css/animations_v2.min.css'
  ]
})
export class ChangePasswordComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;

  userId: string;
  passwordResetCode: string;
  valid: boolean = false;

  changePasswordForm: FormGroup;
  message: string;
  submitted = false;

  constructor(
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
    this.changePasswordForm = this.formBuilder.group({
      password: ['', [Validators.required, Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&]).{6,}')]],
      passwordRepeat: ['']
    }, {validators: ConfirmPasswordValidator('password', 'passwordRepeat')});

    this.blockUI.start('Initialisiere...');

    this.route.queryParams
      .pipe(
        concatMap(params => {
          this.userId = params.userId;
          this.passwordResetCode = params.passwordResetCode;
          return this.authService.validatePasswordResetCodeAsync(this.userId, this.passwordResetCode)
        })
      )
      .subscribe(
        () => {
          this.valid = true;
          this.blockUI.stop();
        },
        (response: HttpErrorResponse) => {
          this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
          this.blockUI.stop();
        },
      );
  }

  get f() {
    return this.changePasswordForm.controls;
  }

  onSubmit(data) {
    this.submitted = true;
    if (this.changePasswordForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.authService.changePasswordWithResetCodeAsync(this.userId, this.passwordResetCode, data.password)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.changePasswordForm.reset();
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
