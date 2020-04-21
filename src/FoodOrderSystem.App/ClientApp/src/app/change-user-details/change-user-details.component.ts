import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { UserAdminService } from '../user/user-admin.service';
import { UserModel } from '../user/user.model';

@Component({
  selector: 'app-change-user-details',
  templateUrl: './change-user-details.component.html',
  styleUrls: ['./change-user-details.component.css']
})
export class ChangeUserDetailsComponent implements OnInit {
  @Input() public user: UserModel;

  changeUserDetailsForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private userAdminService: UserAdminService,
  ) {
  }

  ngOnInit() {
    this.changeUserDetailsForm = this.formBuilder.group({
      name: this.user.name,
      role: this.user.role,
    });
  }

  onSubmit(data) {
    this.userAdminService.changeUserDetailsAsync(this.user.id, data.name, data.role)
      .subscribe(() => {
        this.message = undefined;
        this.changeUserDetailsForm.reset();
        this.activeModal.close('Close click');
      }, (status: number) => {
          if (status === 401)
            this.message = "Sie sind nicht angemdeldet.";
          else if (status === 403)
            this.message = "Sie sind nicht berechtigt, diese Aktion durchzuführen.";
          else
            this.message = "Ein unvorhergesehener Fehler ist aufgetreten. Bitte versuchen Sie es nochmals.";
      });
  }
}
