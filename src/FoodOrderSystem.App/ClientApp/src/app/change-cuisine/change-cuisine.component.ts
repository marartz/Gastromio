import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { CuisineModel } from '../cuisine/cuisine.model';
import { CuisineAdminService } from '../cuisine/cuisine-admin.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-change-cuisine',
  templateUrl: './change-cuisine.component.html',
  styleUrls: ['./change-cuisine.component.css', '../../assets/css/admin-forms.min.css']
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
      name: this.cuisine.name
    });
  }

  onSubmit(data) {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.cuisineAdminService.changeCuisineAsync(this.cuisine.id, data.name)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.changeCuisineForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response);
      });
  }
}
