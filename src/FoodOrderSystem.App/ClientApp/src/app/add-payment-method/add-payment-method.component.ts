import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { PaymentMethodAdminService } from '../payment-method/payment-method-admin.service';

@Component({
  selector: 'app-payment-method',
  templateUrl: './add-payment-method.component.html',
  styleUrls: ['./add-payment-method.component.css']
})
export class AddPaymentMethodComponent implements OnInit {
  addPaymentMethodForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private paymentMethodAdminService: PaymentMethodAdminService,
  ) {
    this.addPaymentMethodForm = this.formBuilder.group({
      name: '',
      description: ''
    });
  }

  ngOnInit() {
  }

  onSubmit(data) {
    this.paymentMethodAdminService.addPaymentMethodAsync(data.name, data.description)
      .subscribe(() => {
        this.message = undefined;
        this.addPaymentMethodForm.reset();
        this.activeModal.close('Close click');
      }, (status: number) => {
          this.addPaymentMethodForm.reset();
          if (status === 401)
            this.message = "Sie sind nicht angemdeldet.";
          else if (status === 403)
            this.message = "Sie sind nicht berechtigt, diese Aktion durchzuführen.";
          else
            this.message = "Ein unvorhergesehener Fehler ist aufgetreten. Bitte versuchen Sie es nochmals.";
      });
  }
}
