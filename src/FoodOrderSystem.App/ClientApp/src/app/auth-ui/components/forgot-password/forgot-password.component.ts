import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {HttpErrorResponse} from "@angular/common/http";

import {take} from "rxjs/operators";

import {HttpErrorHandlingService} from '../../../shared/services/http-error-handling.service';

import {AuthService} from "../../../auth/services/auth.service";

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css', '../../../../assets/css/frontend_v2.min.css', '../../../../assets/css/backend_v2.min.css']
})
export class ForgotPasswordComponent implements OnInit {

  forgotPasswordForm: FormGroup;
  message: string;
  submitted: boolean = false;
  sent: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
    this.forgotPasswordForm = this.formBuilder.group({
      email: ['', [Validators.required]]
    });
  }

  get f() {
    return this.forgotPasswordForm.controls;
  }

  onSubmit(data) {
    if (this.sent)
      return;

    this.submitted = true;
    if (this.forgotPasswordForm.invalid) {
      return;
    }

    this.authService.requestPasswordChangeAsync(data.email)
      .pipe(take(1))
      .subscribe(
        () => {
          this.sent = true;
          this.message = undefined;
        },
        (response: HttpErrorResponse) => {
          this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
        }
      );
  }
}
