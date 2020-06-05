import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { UserAdminService } from '../user/user-admin.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';
import { ConfirmPasswordValidator } from '../validators/password.validator';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css', '../../assets/css/admin-forms.min.css']
})
export class AddUserComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;

  addUserForm: FormGroup;
  message: string;
  submitted = false;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private userAdminService: UserAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
    this.addUserForm = this.formBuilder.group({
      role: ['', Validators.required],
      email: ['', [Validators.required, Validators.email, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]],
      password: ['', [Validators.required, Validators.pattern('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&]).{6,}')]],
      passwordRepeat: ['']
    }, { validators: ConfirmPasswordValidator('password', 'passwordRepeat') });
  }

  ngOnInit() {
  }

  get f() { return this.addUserForm.controls; }

  onSubmit(data) {
    this.submitted = true;
    if (this.addUserForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.userAdminService.addUserAsync(data.role, data.email, data.password)
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
