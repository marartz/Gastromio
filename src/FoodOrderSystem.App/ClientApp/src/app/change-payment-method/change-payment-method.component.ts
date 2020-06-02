import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PaymentMethodModel } from '../payment-method/payment-method.model';
import { PaymentMethodAdminService } from '../payment-method/payment-method-admin.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-change-payment-method',
  templateUrl: './change-payment-method.component.html',
  styleUrls: ['./change-payment-method.component.css', '../../assets/css/admin-forms.min.css']
})
export class ChangePaymentMethodComponent implements OnInit {
  @Input() public paymentMethod: PaymentMethodModel;
  @BlockUI() blockUI: NgBlockUI;

  changePaymentMethodForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private paymentMethodAdminService: PaymentMethodAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
    this.changePaymentMethodForm = this.formBuilder.group({
      name: [this.paymentMethod.name, Validators.required],
      description: [this.paymentMethod.description, Validators.required]
    });
  }

  get f() { return this.changePaymentMethodForm.controls; }

  onSubmit(data) {
    if (this.changePaymentMethodForm.invalid) {
      return;
    }

    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.paymentMethodAdminService.changePaymentMethodAsync(this.paymentMethod.id, data.name, data.description)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.changePaymentMethodForm.reset();
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
