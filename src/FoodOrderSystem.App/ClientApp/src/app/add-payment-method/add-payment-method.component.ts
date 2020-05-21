import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { PaymentMethodAdminService } from '../payment-method/payment-method-admin.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-payment-method',
  templateUrl: './add-payment-method.component.html',
  styleUrls: ['./add-payment-method.component.css', '../../assets/css/admin-forms.min.css']
})
export class AddPaymentMethodComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;

  addPaymentMethodForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private paymentMethodAdminService: PaymentMethodAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
    this.addPaymentMethodForm = this.formBuilder.group({
      name: '',
      description: ''
    });
  }

  ngOnInit() {
  }

  onSubmit(data) {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.paymentMethodAdminService.addPaymentMethodAsync(data.name, data.description)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.addPaymentMethodForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.addPaymentMethodForm.reset();
        this.message = this.httpErrorHandlingService.handleError(response);
      });
  }
}
