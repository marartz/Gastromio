import {Component, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

import {CuisineAdminService} from '../cuisine/cuisine-admin.service';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {HttpErrorResponse} from '@angular/common/http';
import {take} from 'rxjs/operators';

@Component({
  selector: 'app-add-cuisine',
  templateUrl: './add-cuisine.component.html',
  styleUrls: ['./add-cuisine.component.css', '../../assets/css/frontend.min.css', '../../assets/css/backend.min.css']
})
export class AddCuisineComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;

  addCuisineForm: FormGroup;
  message: string;
  submitted = false;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private cuisineAdminService: CuisineAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
    this.addCuisineForm = this.formBuilder.group({
      name: ['', Validators.required]
    });
  }

  ngOnInit() {
  }

  get f() {
    return this.addCuisineForm.controls;
  }

  onSubmit(data) {
    this.submitted = true;
    if (this.addCuisineForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.cuisineAdminService.addCuisineAsync(data.name).pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.addCuisineForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
