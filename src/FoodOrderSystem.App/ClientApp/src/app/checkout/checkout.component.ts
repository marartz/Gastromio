import {Component, OnInit} from '@angular/core';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {OrderService} from '../order/order.service';
import {CartModel} from '../cart/cart.model';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {DishCategoryModel} from '../dish-category/dish-category.model';
import {take} from 'rxjs/operators';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css', '../../assets/css/frontend.min.css']
})
export class CheckoutComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;

  generalError: string;

  initialized: boolean;
  submitted = false;

  restaurant: RestaurantModel;
  dishCategories: DishCategoryModel[];

  customerForm: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private orderService: OrderService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
    this.initialized = false;

    this.blockUI.start();
    this.orderService.initializeAsync()
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();

        const cart = this.orderService.getCart();
        if (!cart) {
          this.generalError = 'Sie haben keine Gerichte im Warenkorb';
          return;
        } else {
          this.generalError = undefined;
        }

        this.customerForm = this.formBuilder.group({
          givenName: ['', Validators.required],
          lastName: ['', Validators.required],
          street: ['', Validators.required],
          zipCode: ['', Validators.required],
          addAddressInfo: [''],
          city: ['', Validators.required],
          phone: ['', Validators.required],
          email: ['', Validators.required, Validators.email, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')],
          comments: ['']
        });

        this.restaurant = this.orderService.getRestaurant();
        this.dishCategories = this.orderService.getDishCategories();
        this.initialized = true;
      }, error => {
        this.blockUI.stop();
        this.generalError = this.httpErrorHandlingService.handleError(error).getJoinedGeneralErrors();
      });
  }

  get f() {
    return this.customerForm.controls;
  }

  getCart(): CartModel {
    return this.orderService.getCart();
  }

  onSubmit(data): void {
    this.submitted = true;
    if (this.customerForm.invalid) {
      return;
    }
  }
}
