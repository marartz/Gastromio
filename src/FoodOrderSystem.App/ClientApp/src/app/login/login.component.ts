import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AuthService } from '../auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css', '../../assets/css/admin-forms.min.css']
})
export class LoginComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;
  loginForm: FormGroup;
  generalError: string;
  submitted = false;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
    this.loginForm = this.formBuilder.group({
      Username: ['', Validators.required],
      Password: ['', Validators.required]
    });
  }

  ngOnInit() {
  }

  get f() { return this.loginForm.controls; }

  onLogin(loginData) {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }

    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.authService.loginAsync(loginData.username, loginData.password)
      .subscribe(() => {
        subscription.unsubscribe();
        this.generalError = undefined;
        this.loginForm.reset();
        this.activeModal.close('Close click');
        this.blockUI.stop();
      }, (error: HttpErrorResponse) => {
          subscription.unsubscribe();
          let errors = this.httpErrorHandlingService.handleError(error);
          this.generalError = errors.getJoinedGeneralErrors();
          errors.addComponentErrorsToFormControls(this.loginForm);
          // don't reset form if there are componentErrors from backend, because
          // otherwise they would also be reset.
          if (!Object.keys(errors.componentErrors).length) {
            this.loginForm.reset();
          }
          this.blockUI.stop();
      });
  }
}
