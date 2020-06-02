import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserAdminService } from '../user/user-admin.service';
import { UserModel } from '../user/user.model';
import { Subject, Subscription } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AddUserComponent } from '../add-user/add-user.component';
import { ChangeUserDetailsComponent } from '../change-user-details/change-user-details.component';
import { ChangeUserPasswordComponent } from '../change-user-password/change-user-password.component';
import { RemoveUserComponent } from '../remove-user/remove-user.component';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.css', '../../assets/css/admin.min.css']
})
export class AdminUsersComponent implements OnInit, OnDestroy {
  users: UserModel[];
  pageOfUsers: UserModel[];

  private searchPhrase: string;
  private searchPhraseUpdated: Subject<string> = new Subject<string>();

  private updateSearchSubscription: Subscription;

  constructor(
    private modalService: NgbModal,
    private userAdminService: UserAdminService
  ) {
    this.searchPhraseUpdated.asObservable().pipe(debounceTime(200), distinctUntilChanged())
      .subscribe((value: string) => {
        this.searchPhrase = value;
        this.updateSearch();
      });
  }

  ngOnInit() {
    this.searchPhrase = '';
    this.updateSearch();
  }

  ngOnDestroy() {
    if (this.updateSearchSubscription !== undefined) {
      this.updateSearchSubscription.unsubscribe();
    }
  }

  openAddUserForm(): void {
    const modalRef = this.modalService.open(AddUserComponent);
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => { });
  }

  openChangeUserDetailsForm(user: UserModel): void {
    const modalRef = this.modalService.open(ChangeUserDetailsComponent);
    modalRef.componentInstance.user = user;
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => {});
  }

  openChangeUserPasswordForm(user: UserModel): void {
    const modalRef = this.modalService.open(ChangeUserPasswordComponent);
    modalRef.componentInstance.user = user;
  }

  openRemoveUserForm(user: UserModel): void {
    const modalRef = this.modalService.open(RemoveUserComponent);
    modalRef.componentInstance.user = user;
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => { });
  }

  onSearchType(value: string) {
    this.searchPhraseUpdated.next(value);
  }

  onChangePage(pageOfUsers: UserModel[]) {
    this.pageOfUsers = pageOfUsers;
  }

  updateSearch(): void {
    if (this.updateSearchSubscription !== undefined) {
      this.updateSearchSubscription.unsubscribe();
    }

    const observable = this.userAdminService.searchForUsersAsync(this.searchPhrase);

    this.updateSearchSubscription = observable.subscribe((result) => {
      this.users = result;
    }, () => {
    });
  }

  localizeRole(role: string): string {
    switch (role) {
      case 'SystemAdmin':
        return 'Systemadministrator';
      case 'RestaurantAdmin':
        return 'Restaurantadministrator';
      case 'Customer':
        return 'Kunde';
      default:
        return 'Kunde';
    }
  }
}
