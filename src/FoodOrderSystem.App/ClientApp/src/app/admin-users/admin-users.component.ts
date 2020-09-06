import {Component, OnInit, OnDestroy, ViewChild, AfterViewInit} from '@angular/core';
import {UserAdminService} from '../user/user-admin.service';
import {UserModel} from '../user/user.model';
import {Subject} from 'rxjs';
import {debounceTime, distinctUntilChanged, take} from 'rxjs/operators';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {AddUserComponent} from '../add-user/add-user.component';
import {ChangeUserDetailsComponent} from '../change-user-details/change-user-details.component';
import {ChangeUserPasswordComponent} from '../change-user-password/change-user-password.component';
import {RemoveUserComponent} from '../remove-user/remove-user.component';
import {FetchPageInfo, ServerPaginationComponent} from '../pagination/server-pagination.component';

@Component({
  selector: 'app-admin-users',
  templateUrl: './admin-users.component.html',
  styleUrls: [
    './admin-users.component.css',
    '../../assets/css/frontend_v2.min.css',
    '../../assets/css/backend_v2.min.css',
    '../../assets/css/animations_v2.min.css'
  ]
})
export class AdminUsersComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(ServerPaginationComponent) pagingComponent: ServerPaginationComponent;
  pageOfUsers: UserModel[];

  private searchPhrase: string;
  private searchPhraseUpdated: Subject<string> = new Subject<string>();

  constructor(
    private modalService: NgbModal,
    private userAdminService: UserAdminService
  ) {
    this.searchPhraseUpdated.asObservable().pipe(debounceTime(200), distinctUntilChanged())
      .subscribe((value: string) => {
        this.searchPhrase = value;
        this.pagingComponent.triggerFetchPage(1);
      });
  }

  ngOnInit() {
    this.searchPhrase = '';
  }

  ngAfterViewInit() {
    // ViewChild has to be rendered before it can be accessed
    this.pagingComponent.triggerFetchPage(1);
  }

  ngOnDestroy() {
    this.searchPhraseUpdated?.unsubscribe();
  }

  onUpdateSearch(value: string) {
    this.searchPhraseUpdated.next(value);
  }

  onFetchPage(info: FetchPageInfo) {
    this.userAdminService.searchForUsersAsync(this.searchPhrase, info.skip, info.take)
      .pipe(take(1))
      .subscribe((result) => {
        this.pageOfUsers = result.items;
        this.pagingComponent.updatePaging(result.total, result.items.length);
      }, () => {
      });
  }

  openAddUserForm(): void {
    const modalRef = this.modalService.open(AddUserComponent);
    modalRef.result.then(() => {
      this.pagingComponent.triggerFetchPage();
    }, () => {
    });
  }

  openChangeUserDetailsForm(user: UserModel): void {
    const modalRef = this.modalService.open(ChangeUserDetailsComponent);
    modalRef.componentInstance.user = user;
    modalRef.result.then(() => {
      this.pagingComponent.triggerFetchPage();
    }, () => {
    });
  }

  openChangeUserPasswordForm(user: UserModel): void {
    const modalRef = this.modalService.open(ChangeUserPasswordComponent);
    modalRef.componentInstance.user = user;
  }

  openRemoveUserForm(user: UserModel): void {
    const modalRef = this.modalService.open(RemoveUserComponent);
    modalRef.componentInstance.user = user;
    modalRef.result.then(() => {
      this.pagingComponent.triggerFetchPage();
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