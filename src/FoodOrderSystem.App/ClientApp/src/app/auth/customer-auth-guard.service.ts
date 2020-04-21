import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable()
export class CustomerAuthGuardService implements CanActivate {

  constructor(
    public auth: AuthService,
    public router: Router
  ) { }

  canActivate(): boolean {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(['']);
      return false;
    }

    let currentUser = this.auth.getUser();
    if (currentUser === undefined || (currentUser.role !== "SystemAdmin" && currentUser.role !== "RestaurantAdmin" && currentUser.role !== "Customer")) {
      this.router.navigate(['']);
      return false;
    }

    return true;
  }
}
