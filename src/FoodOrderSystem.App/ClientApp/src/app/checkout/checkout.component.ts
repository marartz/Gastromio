import {Component, OnInit} from '@angular/core';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {OrderService} from '../order/order.service';
import {CartModel} from '../cart/cart.model';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {DishCategoryModel} from '../dish-category/dish-category.model';
import {take} from 'rxjs/operators';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {CheckoutModel, OrderedDishInfoModel} from '../order/checkout.model';
import {HttpErrorResponse} from '@angular/common/http';

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

    const cart = this.getCart();

    const checkoutModel = new CheckoutModel();
    checkoutModel.givenName = data.givenName;
    checkoutModel.lastName = data.lastName;
    checkoutModel.street = data.street;
    checkoutModel.addAddressInfo = data.addAddressInfo;
    checkoutModel.zipCode = data.zipCode;
    checkoutModel.city = data.city;
    checkoutModel.phone = data.phone;
    checkoutModel.email = data.email;
    checkoutModel.orderType = OrderService.translateFromOrderType(cart.getOrderType());
    checkoutModel.restaurantId = cart.getRestaurantId();
    checkoutModel.orderedDishes = new Array<OrderedDishInfoModel>();

    for (const orderedDish of cart.getOrderedDishes()) {
      const orderedDishInfo = new OrderedDishInfoModel();
      orderedDishInfo.itemId = orderedDish.getItemId();
      orderedDishInfo.dishId = orderedDish.getDish().id;
      orderedDishInfo.variantId = orderedDish.getVariant().variantId;
      orderedDishInfo.count = orderedDish.getCount();
      orderedDishInfo.remarks = orderedDish.getRemarks();
      checkoutModel.orderedDishes.push(orderedDishInfo);
    }

    checkoutModel.comments = data.comments;

    this.blockUI.start('Verarbeite Daten...');
    this.orderService.checkoutAsync(checkoutModel)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.generalError = undefined;
        this.customerForm.reset();
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
