import {Component, OnInit} from '@angular/core';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {OrderService} from '../order/order.service';
import {CartModel} from '../cart/cart.model';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {DishCategoryModel} from '../dish-category/dish-category.model';
import {take} from 'rxjs/operators';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {CheckoutModel} from '../order/checkout.model';
import {HttpErrorResponse} from '@angular/common/http';
import {StoredCartDishModel} from '../cart/stored-cart-dish.model';
import {Router} from '@angular/router';
import {Location} from '@angular/common';
import {CartDishModel} from "../cart/cart-dish.model";

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: [
    './checkout.component.css',
    '../../assets/css/frontend_v3.min.css'
  ]
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
    private httpErrorHandlingService: HttpErrorHandlingService,
    private router: Router,
    private location: Location
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
          phone: ['', [Validators.required, Validators.pattern(/^(((((((00|\+)49[ \-/]?)|0)[1-9][0-9]{1,4})[ \-/]?)|((((00|\+)49\()|\(0)[1-9][0-9]{1,4}\)[ \-/]?))[0-9]{1,7}([ \-/]?[0-9]{1,5})?)$/)]],
          email: ['', [Validators.required, Validators.email, Validators.pattern('^[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}$')]],
          comments: [''],
          paymentMethodId: ['', Validators.required]
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

  hasLogo(): boolean {
    return this.restaurant?.imageTypes.some(en => en === 'logo');
  }

  getLogoUrl(): string {
    if (!this.restaurant) {
      return undefined;
    }
    return '/api/v1/restaurants/' + this.restaurant.id + '/images/logo';
  }

  hasBanner(
    restaurant: RestaurantModel
  ): boolean {
    return restaurant.imageTypes.some(en => en === 'banner');
  }

  getBannerStyle(
    restaurant: RestaurantModel
  ): string {
    if (!restaurant) {
      return undefined;
    }
    return '/api/v1/restaurants/' + restaurant.id + '/images/banner';
  }

  getCart(): CartModel {
    return this.orderService.getCart();
  }

  onBack(): void {
    this.location.back();
  }

  public onRemoveDishVariantFromCart(cartDishVariant: CartDishModel): void {
    if (cartDishVariant === undefined) {
      return;
    }
    this.orderService.removeCartDishFromCart(cartDishVariant.getItemId());
  }

  isValid(): boolean {
    return !this.customerForm.invalid;
  }

  onSubmit(data): void {
    console.log('onSubmit: ', data);

    this.submitted = true;

    if (!this.isValid()) {
      return;
    }

    const cart = this.getCart();

    const checkoutModel = new CheckoutModel();
    checkoutModel.givenName = data.givenName;
    checkoutModel.lastName = data.lastName;
    checkoutModel.street = data.street;
    checkoutModel.addAddressInfo = data.addAddressInfo;
    checkoutModel.zipCode = data.zipCode.toString();
    checkoutModel.city = data.city;
    checkoutModel.phone = data.phone;
    checkoutModel.email = data.email;
    checkoutModel.orderType = OrderService.translateFromOrderType(cart.getOrderType());
    checkoutModel.restaurantId = cart.getRestaurantId();
    checkoutModel.cartDishes = new Array<StoredCartDishModel>();

    for (const cartDish of cart.getCartDishes()) {
      const storedCartDish = new StoredCartDishModel();
      storedCartDish.itemId = cartDish.getItemId();
      storedCartDish.dishId = cartDish.getDish().id;
      storedCartDish.variantId = cartDish.getVariant().variantId;
      storedCartDish.count = cartDish.getCount();
      storedCartDish.remarks = cartDish.getRemarks();
      checkoutModel.cartDishes.push(storedCartDish);
    }

    checkoutModel.comments = data.comments;
    checkoutModel.paymentMethodId = data.paymentMethodId;

    console.log('Checkout Model: ', checkoutModel);

    this.blockUI.start('Bestellung wird verarbeitet...');
    this.orderService.checkoutAsync(checkoutModel)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.generalError = undefined;
        this.customerForm.reset();
        this.router.navigateByUrl('/order-summary');
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.generalError = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
