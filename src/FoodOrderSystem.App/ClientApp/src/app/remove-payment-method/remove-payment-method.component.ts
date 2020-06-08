import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PaymentMethodModel } from '../payment-method/payment-method.model';
import { PaymentMethodAdminService } from '../payment-method/payment-method-admin.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-remove-payment-method',
  templateUrl: './remove-payment-method.component.html',
  styleUrls: ['./remove-payment-method.component.css', '../../assets/css/frontend.min.css']
})
export class RemovePaymentMethodComponent implements OnInit {
  @Input() public paymentMethod: PaymentMethodModel;
  @BlockUI() blockUI: NgBlockUI;

  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private paymentMethodAdminService: PaymentMethodAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
  }

  onSubmit() {
    this.blockUI.start('Verarbeite Daten...');
    const subscription = this.paymentMethodAdminService.removePaymentMethodAsync(this.paymentMethod.id)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
