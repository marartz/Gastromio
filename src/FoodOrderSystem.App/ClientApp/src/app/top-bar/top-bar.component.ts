import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

import { AuthService } from '../auth/auth.service';
import { LoginComponent } from '../login/login.component';
import { UserModel } from '../user/user.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent implements OnInit {

  constructor(
    private modalService: NgbModal,
    private authService: AuthService,
    private router: Router,
  ) { }

  ngOnInit() {
  }

  getUsername(): string {
    let currentUser: UserModel = this.authService.getUser();
    return currentUser !== undefined ? currentUser.name : undefined;
  }

  systemAdminAllowed(): boolean {
    let currentUser: UserModel = this.authService.getUser();
    return currentUser !== undefined && currentUser.role !== undefined && currentUser.role === "SystemAdmin";
  }

  restaurantAdminAllowed(): boolean {
    let currentUser: UserModel = this.authService.getUser();
    return currentUser !== undefined && currentUser.role !== undefined && (currentUser.role === "SystemAdmin" || currentUser.role === "RestaurantAdmin");
  }

  openLoginForm(): void {
    this.modalService.open(LoginComponent);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['']);
  }
}
