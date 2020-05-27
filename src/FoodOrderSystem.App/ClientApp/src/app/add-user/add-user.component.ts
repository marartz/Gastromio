import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { UserAdminService } from '../user/user-admin.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css', '../../assets/css/admin-forms.min.css']
})
export class AddUserComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;

  addUserForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private userAdminService: UserAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
    this.addUserForm = this.formBuilder.group({
      username: '',
      role: '',
      email: '',
      password: '',
      passwordRepeat: ''
    });
  }

  ngOnInit() {
  }

  onSubmit(data) {
    if (data.password === undefined || data.password === '') {
      this.message = 'Bitte geben Sie ein Passwort ein';
      return;
    }

    if (data.passwordRepeat === undefined || data.passwordRepeat === '') {
      this.message = 'Bitte wiederholen Sie das eingegebene Passwort';
      return;
    }

    if (data.password !== data.passwordRepeat) {
      this.message = 'Die eingegebenen Passwörter stimmen nicht überein';
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.userAdminService.addUserAsync(data.username, data.role, data.email, data.password)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.addUserForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.addUserForm.reset();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
