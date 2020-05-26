import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css', '../../assets/css/admin-forms.min.css']
})
export class LoginComponent implements OnInit {
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
    this.authService.loginAsync(loginData.username, loginData.password)
      .subscribe(() => {
        this.message = undefined;
        this.loginForm.reset();
        this.activeModal.close('Close click');
      }, (status: number) => {
          this.loginForm.reset();
          if (status === 401)
            this.message = "Benutzername und/oder Passwort ist nicht korrekt.";
          else
            this.message = "Ein Fehler ist aufgetreten. Bitte versuchen Sie es erneut.";
      });
  }
}
