import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable()
export class RestaurantAdminAuthGuardService implements CanActivate {

  constructor(
    public auth: AuthService,
    public router: Router
  ) { }

  canActivate(): boolean {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(['']);
      return false;
    }

    const currentUser = this.auth.getUser();
    if (currentUser === undefined || (currentUser.role !== 'SystemAdmin' && currentUser.role !== 'RestaurantAdmin')) {
      this.router.navigate(['']);
      return false;
    } else {
      const subscription = this.auth.pingAsync().subscribe(() => {
        subscription.unsubscribe();
      }, () => {
        subscription.unsubscribe();
        this.auth.logout();
        this.router.navigate(['']);
      });
    }

    return true;
  }
}
