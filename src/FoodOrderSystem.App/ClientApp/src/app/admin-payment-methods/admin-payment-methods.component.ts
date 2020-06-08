import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PaymentMethodAdminService } from '../payment-method/payment-method-admin.service';
import { Observable } from 'rxjs';
import { PaymentMethodModel } from '../payment-method/payment-method.model';
import { AddPaymentMethodComponent } from '../add-payment-method/add-payment-method.component';
import { ChangePaymentMethodComponent } from '../change-payment-method/change-payment-method.component';
import { RemovePaymentMethodComponent } from '../remove-payment-method/remove-payment-method.component';

@Component({
  selector: 'app-admin-payment-methods',
  templateUrl: './admin-payment-methods.component.html',
  styleUrls: ['./admin-payment-methods.component.css', '../../assets/css/frontend.min.css']
})
export class AdminPaymentMethodsComponent implements OnInit {
  paymentMethods: Observable<PaymentMethodModel[]>;

  constructor(
    private modalService: NgbModal,
    private paymentMethodAdminService: PaymentMethodAdminService
  ) { }

  ngOnInit() {
    this.updateSearch();
  }

  openAddPaymentMethodForm(): void {
    const modalRef = this.modalService.open(AddPaymentMethodComponent);
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => { });
  }

  openChangePaymentMethodForm(paymentMethod: PaymentMethodModel): void {
    const modalRef = this.modalService.open(ChangePaymentMethodComponent);
    modalRef.componentInstance.paymentMethod = paymentMethod;
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => { });
  }

  openRemovePaymentMethodForm(paymentMethod: PaymentMethodModel): void {
    const modalRef = this.modalService.open(RemovePaymentMethodComponent);
    modalRef.componentInstance.paymentMethod = paymentMethod;
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => { });
  }

  updateSearch(): void {
    this.paymentMethods = this.paymentMethodAdminService.getAllPaymentMethodsAsync();
  }
}
