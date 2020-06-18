import {Component, OnInit} from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

import {AuthService} from '../auth/auth.service';
import {LoginComponent} from '../login/login.component';
import {UserModel} from '../user/user.model';
import {Router} from '@angular/router';
import {OrderService} from '../order/order.service';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css', '../../assets/css/frontend.min.css']
})
export class TopBarComponent implements OnInit {

  constructor(
    private modalService: NgbModal,
    private authService: AuthService,
    private router: Router,
    private orderService: OrderService
  ) {
  }

  ngOnInit() {
  }

  getUserEmail(): string {
    const currentUser: UserModel = this.authService.getUser();
    return currentUser !== undefined ? currentUser.email : undefined;
  }

  getUserRole(): string {
    const currentUser: UserModel = this.authService.getUser();
    return currentUser !== undefined ? currentUser.role : undefined;
  }

  isSystemAdmin(): boolean {
    const currentUser: UserModel = this.authService.getUser();
    return currentUser !== undefined && currentUser.role !== undefined && currentUser.role === 'SystemAdmin';
  }

  isRestaurantAdmin(): boolean {
    const currentUser: UserModel = this.authService.getUser();
    return currentUser !== undefined && currentUser.role !== undefined && currentUser.role === 'RestaurantAdmin';
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['']);
  }

  getDishCountOfOrderText(): string {
    const count = this.orderService.getCart()?.getDishCountOfOrder();
    if (!count) {
      return '0';
    }
    return count.toString();
  }

  isOnOrderPage(): boolean {
    return this.router.url.startsWith('/restaurants/');
  }

  getCurRestaurantId(): string {
    const cart = this.orderService.getCart();
    return cart?.getRestaurantId();
  }

  toggleCartVisibility(): void {
    const cart = this.orderService.getCart();
    if (!cart) {
      return;
    }
    if (cart.isVisible()) {
      this.orderService.hideCart();
    } else {
      this.orderService.showCart();
    }
  }
}
