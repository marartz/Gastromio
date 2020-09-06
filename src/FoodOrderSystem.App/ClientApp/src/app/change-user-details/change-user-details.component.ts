import {Component, OnInit, Input} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

import {UserAdminService} from '../user/user-admin.service';
import {UserModel} from '../user/user.model';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {HttpErrorResponse} from '@angular/common/http';
import {take} from 'rxjs/operators';

@Component({
  selector: 'app-change-user-details',
  templateUrl: './change-user-details.component.html',
  styleUrls: ['./change-user-details.component.css', '../../assets/css/frontend_v2.min.css', '../../assets/css/backend_v2.min.css']
})
export class ChangeUserDetailsComponent implements OnInit {
  @Input() public user: UserModel;
  @BlockUI() blockUI: NgBlockUI;

  changeUserDetailsForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private userAdminService: UserAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService,
  ) {
  }

  ngOnInit() {
    this.changeUserDetailsForm = this.formBuilder.group({
      role: [this.user.role, Validators.required],
      email: [this.user.email, [Validators.required, Validators.email, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]]
    });
  }

  get f() {
    return this.changeUserDetailsForm.controls;
  }

  onSubmit(data) {
    if (this.changeUserDetailsForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.userAdminService.changeUserDetailsAsync(this.user.id, data.role, data.email)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.changeUserDetailsForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}