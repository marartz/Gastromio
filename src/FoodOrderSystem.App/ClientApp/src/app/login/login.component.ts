import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { AuthService } from '../auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import {ActivatedRoute, Router} from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css', '../../assets/css/frontend.min.css']
})
export class LoginComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;
  loginForm: FormGroup;
  generalError: string;
  submitted = false;
  returnUrl = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
    this.loginForm = this.formBuilder.group({
      Email: ['', Validators.required],
      Password: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {
      const tempReturnUrl = params.get('returnUrl');
      if (!this.isAbsoluteUrl(tempReturnUrl)) {
        this.returnUrl = tempReturnUrl;
      }
    });
  }

  get f() { return this.loginForm.controls; }

  onLogin(loginData) {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.authService.loginAsync(loginData.Email, loginData.Password)
      .subscribe(() => {
        subscription.unsubscribe();
        this.generalError = undefined;
        this.loginForm.reset();
        this.blockUI.stop();
        this.router.navigateByUrl(this.returnUrl);
      }, (error: HttpErrorResponse) => {
          subscription.unsubscribe();
          const errors = this.httpErrorHandlingService.handleError(error);
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

  isAbsoluteUrl(url: string): boolean {
    const r = new RegExp('^(?:[a-z]+:)?//', 'i');
    return r.test(url);
  }
}
