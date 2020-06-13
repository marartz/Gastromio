import {Injectable} from '@angular/core';
import {Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot} from '@angular/router';
import {AuthService} from './auth.service';
import {take} from 'rxjs/operators';

@Injectable()
export class SystemAdminAuthGuardService implements CanActivate {

  constructor(
    public auth: AuthService,
    public router: Router
  ) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    const loginUrl = '/login?returnUrl=' + encodeURIComponent(state.url);

    if (!this.auth.isAuthenticated()) {
      this.router.navigateByUrl(loginUrl);
      return false;
    }

    const currentUser = this.auth.getUser();
    if (currentUser === undefined || currentUser.role !== 'SystemAdmin') {
      this.router.navigateByUrl(loginUrl);
      return false;
    } else {
      this.auth.pingAsync()
        .pipe(take(1))
        .subscribe(() => {
        }, () => {
          this.auth.logout();
          this.router.navigateByUrl(loginUrl);
        });
    }

    return true;
  }
}
