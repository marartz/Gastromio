import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {AuthService} from "../auth/auth.service";
import {take} from "rxjs/operators";
import {HttpErrorResponse} from "@angular/common/http";

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css', '../../assets/css/frontend_v2.min.css', '../../assets/css/backend_v2.min.css']
})
export class ForgotPasswordComponent implements OnInit {

  forgotPasswordForm: FormGroup;
  message: string;
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

  onSubmit(data) {
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
