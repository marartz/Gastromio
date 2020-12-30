import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

import {take} from 'rxjs/operators';

import {UserModel} from '../../../shared/models/user.model';

import {AuthService} from '../../../auth/services/auth.service';

import {OrderFacade} from "../../../order/order.facade";

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: [
    './top-bar.component.css',
    '../../../../assets/css/frontend_v3.min.css'
  ]
})
export class TopBarComponent implements OnInit {

  constructor(
    private modalService: NgbModal,
    private authService: AuthService,
    private router: Router,
    private orderFacade: OrderFacade
  ) {
  }

  ngOnInit() {
    this.orderFacade.initialize();
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

  isOnOrderPage(): boolean {
    return this.router.url.startsWith('/restaurants/');
  }

  getCurRestaurantId(): string {
    const cart = this.orderFacade.getCart();
    return cart?.getRestaurantId();
  }

  toggleCartVisibility(): void {
    const cart = this.orderFacade.getCart();
    if (!cart) {
      return;
    }
    if (cart.isVisible()) {
      this.orderFacade.hideCart();
    } else {
      this.orderFacade.showCart();
    }
  }
}
