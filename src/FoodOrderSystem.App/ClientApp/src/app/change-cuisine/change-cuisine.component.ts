import {Component, OnInit, Input} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

import {CuisineModel} from '../cuisine/cuisine.model';
import {CuisineAdminService} from '../cuisine/cuisine-admin.service';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {HttpErrorResponse} from '@angular/common/http';
import {take} from 'rxjs/operators';

@Component({
  selector: 'app-change-cuisine',
  templateUrl: './change-cuisine.component.html',
  styleUrls: [
    './change-cuisine.component.css',
    '../../assets/css/frontend_v2.min.css',
    '../../assets/css/backend_v2.min.css',
    '../../assets/css/animations_v2.min.css'
  ]
})
export class ChangeCuisineComponent implements OnInit {
  @Input() public cuisine: CuisineModel;
  @BlockUI() blockUI: NgBlockUI;

  changeCuisineForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private cuisineAdminService: CuisineAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
    this.changeCuisineForm = this.formBuilder.group({
      name: [this.cuisine.name, Validators.required]
    });
  }

  get f() {
    return this.changeCuisineForm.controls;
  }

  onSubmit(data) {
    if (this.changeCuisineForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    this.cuisineAdminService.changeCuisineAsync(this.cuisine.id, data.name)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.changeCuisineForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}