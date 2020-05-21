import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { UserAdminService } from '../user/user-admin.service';
import { UserModel } from '../user/user.model';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-change-user-password',
  templateUrl: './change-user-password.component.html',
  styleUrls: ['./change-user-password.component.css', '../../assets/css/admin-forms.min.css']
})
export class ChangeUserPasswordComponent implements OnInit {
  @Input() public user: UserModel;
  @BlockUI() blockUI: NgBlockUI;

  changeUserPasswordForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private userAdminService: UserAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
    this.changeUserPasswordForm = this.formBuilder.group({
      password: "",
      passwordRepeat: "",
    });
  }

  onSubmit(data) {
    if (data.password === undefined || data.password === "") {
      this.message = "Bitte geben Sie ein Passwort ein";
      return;
    }

    if (data.passwordRepeat === undefined || data.passwordRepeat === "") {
      this.message = "Bitte wiederholen Sie das eingegebene Passwort";
      return;
    }

    if (data.password !== data.passwordRepeat) {
      this.message = "Die eingegebenen Passwörter stimmen nicht überein";
      return;
    }

    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.userAdminService.changeUserPasswordAsync(this.user.id, data.password)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.changeUserPasswordForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response);
      });
  }
}
