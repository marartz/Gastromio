import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { UserAdminService } from '../user/user-admin.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {
  addUserForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private userAdminService: UserAdminService,
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

    this.userAdminService.addUserAsync(data.username, data.role, data.email, data.password)
      .subscribe(() => {
        this.message = undefined;
        this.addUserForm.reset();
        this.activeModal.close('Close click');
      }, (status: number) => {
          this.addUserForm.reset();
          if (status === 401)
            this.message = "Sie sind nicht angemdeldet.";
          else if (status === 403)
            this.message = "Sie sind nicht berechtigt, diese Aktion durchzuführen.";
          else
            this.message = "Ein unvorhergesehener Fehler ist aufgetreten. Bitte versuchen Sie es nochmals.";
      });
  }
}
