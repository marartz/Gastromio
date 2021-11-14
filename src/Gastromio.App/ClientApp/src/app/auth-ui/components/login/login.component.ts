import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { UserModel } from '../../../shared/models/user.model';
import { HttpErrorHandlingService } from '../../../shared/services/http-error-handling.service';

import { AuthService } from '../../../auth/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: [
    './login.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/application-ui/forms/sign-in-and-registration.min.css',
  ],
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
      Password: ['', Validators.required],
    });
  }

  ngOnInit() {
    this.route.queryParamMap.subscribe((params) => {
      const tempReturnUrl = params.get('returnUrl');
      if (!this.isAbsoluteUrl(tempReturnUrl)) {
        this.returnUrl = tempReturnUrl;
      }
    });
  }

  get f() {
    return this.loginForm.controls;
  }

  onLogin(loginData) {
    this.submitted = true;
    if (this.loginForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.authService.loginAsync(loginData.Email, loginData.Password).subscribe(
      (user: UserModel) => {
        this.generalError = undefined;
        this.loginForm.reset();

        this.blockUI.stop();

        if (this.returnUrl && this.returnUrl.trim() !== '') {
          this.router.navigateByUrl(this.returnUrl);
        } else {
          if (user.role === 'SystemAdmin') {
            this.router.navigateByUrl('admin/users');
          } else if (user.role === 'RestaurantAdmin') {
            this.router.navigateByUrl('admin/myrestaurants');
          } else {
            this.router.navigateByUrl('');
          }
        }
      },
      (error: HttpErrorResponse) => {
        const errors = this.httpErrorHandlingService.handleError(error);
        this.generalError = errors.message;
        this.blockUI.stop();
      }
    );
  }

  isAbsoluteUrl(url: string): boolean {
    const r = new RegExp('^(?:[a-z]+:)?//', 'i');
    return r.test(url);
  }
}
