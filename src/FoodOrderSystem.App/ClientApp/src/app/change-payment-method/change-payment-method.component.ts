import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';
import { PaymentMethodModel } from '../payment-method/payment-method.model';
import { PaymentMethodAdminService } from '../payment-method/payment-method-admin.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

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
  ) {
  }

  ngOnInit() {
    this.changePaymentMethodForm = this.formBuilder.group({
      name: this.paymentMethod.name,
      description: this.paymentMethod.description
    });
  }

  onSubmit(data) {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.paymentMethodAdminService.changePaymentMethodAsync(this.paymentMethod.id, data.name, data.description)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.changePaymentMethodForm.reset();
        this.activeModal.close('Close click');
      }, (status: number) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        if (status === 401)
          this.message = "Sie sind nicht angemdeldet.";
        else if (status === 403)
          this.message = "Sie sind nicht berechtigt, diese Aktion durchzuführen.";
        else
          this.message = "Ein unvorhergesehener Fehler ist aufgetreten. Bitte versuchen Sie es nochmals.";
      });
  }
}
