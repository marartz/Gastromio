import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { AuthService } from '../auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css', '../../assets/css/admin-forms.min.css']
})
export class LoginComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;
  loginForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private authService: AuthService,
  ) {
    this.loginForm = this.formBuilder.group({
      username: '',
      password: ''
    });
  }

  ngOnInit() {
  }

  onLogin(loginData) {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.authService.loginAsync(loginData.username, loginData.password)
      .subscribe(() => {
        subscription.unsubscribe();
        this.message = undefined;
        this.loginForm.reset();
        this.activeModal.close('Close click');
        this.blockUI.stop();
      }, (error: HttpErrorResponse) => {
          subscription.unsubscribe();
          this.loginForm.reset();
          this.message = error.error;
          this.blockUI.stop();
      });
  }
}
