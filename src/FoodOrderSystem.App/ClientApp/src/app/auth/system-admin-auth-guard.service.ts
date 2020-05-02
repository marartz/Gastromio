import { Injectable } from '@angular/core';
import { Router, CanActivate } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable()
export class SystemAdminAuthGuardService implements CanActivate {

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
    if (currentUser === undefined || currentUser.role !== "SystemAdmin") {
      this.router.navigate(['']);
      return false;
    } else {
      let subscription = this.auth.pingAsync().subscribe(() => {
        subscription.unsubscribe();
      }, (error) => {
        subscription.unsubscribe();
        this.auth.logout();
        this.router.navigate(['']);
      });
    }

    return true;
  }
}
