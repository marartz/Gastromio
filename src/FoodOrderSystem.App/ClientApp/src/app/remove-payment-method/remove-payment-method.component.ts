import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PaymentMethodModel } from '../payment-method/payment-method.model';
import { PaymentMethodAdminService } from '../payment-method/payment-method-admin.service';

@Component({
  selector: 'app-remove-payment-method',
  templateUrl: './remove-payment-method.component.html',
  styleUrls: ['./remove-payment-method.component.css']
})
export class RemovePaymentMethodComponent implements OnInit {
  @Input() public paymentMethod: PaymentMethodModel;

  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private paymentMethodAdminService: PaymentMethodAdminService,
  ) {
  }

  ngOnInit() {
  }

  onSubmit() {
    this.paymentMethodAdminService.removePaymentMethodAsync(this.paymentMethod.id)
      .subscribe(() => {
        this.message = undefined;
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
