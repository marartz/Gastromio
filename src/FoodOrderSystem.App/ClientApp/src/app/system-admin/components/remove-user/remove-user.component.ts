import {Component, OnInit, Input} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {take} from 'rxjs/operators';

import {UserModel} from '../../../shared/models/user.model';

import {HttpErrorHandlingService} from '../../../shared/services/http-error-handling.service';

import {UserAdminService} from '../../services/user-admin.service';

@Component({
  selector: 'app-remove-user',
  templateUrl: './remove-user.component.html',
  styleUrls: [
    './remove-user.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class RemoveUserComponent implements OnInit {
  @Input() public user: UserModel;
  @BlockUI() blockUI: NgBlockUI;

  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private userAdminService: UserAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
  }

  onSubmit() {
    this.blockUI.start('Verarbeite Daten...');
    this.userAdminService.removeUserAsync(this.user.id)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
