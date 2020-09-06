import {Component, OnInit, OnDestroy, ViewChild, AfterViewInit} from '@angular/core';
import {Subject} from 'rxjs';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {debounceTime, distinctUntilChanged, take} from 'rxjs/operators';

import {RestaurantSysAdminService} from '../restaurant-sys-admin/restaurant-sys-admin.service';
import {AddRestaurantComponent} from '../add-restaurant/add-restaurant.component';
import {RemoveRestaurantComponent} from '../remove-restaurant/remove-restaurant.component';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {ServerPaginationComponent, FetchPageInfo} from '../pagination/server-pagination.component';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {RestaurantRestAdminService} from '../restaurant-rest-admin/restaurant-rest-admin.service';

@Component({
  selector: 'app-admin-restaurants',
  templateUrl: './admin-restaurants.component.html',
  styleUrls: ['./admin-restaurants.component.css', '../../assets/css/frontend_v2.min.css', '../../assets/css/backend_v2.min.css']
})
export class AdminRestaurantsComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild(ServerPaginationComponent) pagingComponent: ServerPaginationComponent;
  pageOfRestaurants: RestaurantModel[];

  private searchPhrase: string;
  private searchPhraseUpdated: Subject<string> = new Subject<string>();

  constructor(
    private modalService: NgbModal,
    private restaurantSysAdminService: RestaurantSysAdminService,
    private restaurantRestAdminService: RestaurantRestAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
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
    this.restaurantSysAdminService.searchForRestaurantsAsync(this.searchPhrase, info.skip, info.take)
      .pipe(take(1))
      .subscribe((result) => {
        this.pageOfRestaurants = result.items;
        this.pagingComponent.updatePaging(result.total, result.items.length);
      }, () => {
      });
  }

  openAddRestaurantForm(): void {
    const modalRef = this.modalService.open(AddRestaurantComponent);
    modalRef.result.then(() => {
      this.pagingComponent.triggerFetchPage();
    }, () => {
    });
  }

  openRemoveRestaurantForm(restaurant: RestaurantModel): void {
    const modalRef = this.modalService.open(RemoveRestaurantComponent);
    modalRef.componentInstance.restaurant = restaurant;
    modalRef.result.then(() => {
      this.pagingComponent.triggerFetchPage();
    }, () => {
    });
  }

  onActivate(restaurant: RestaurantModel): void {
    this.restaurantRestAdminService.activateRestaurantAsync(restaurant.id)
      .pipe(take(1))
      .subscribe(() => {
        restaurant.isActive = true;
      }, response => {
        alert(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

  onDeactivate(restaurant: RestaurantModel): void {
    this.restaurantRestAdminService.deactivateRestaurantAsync(restaurant.id)
      .pipe(take(1))
      .subscribe(() => {
        restaurant.isActive = false;
      }, response => {
        alert(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

  onEnableSupport(restaurant: RestaurantModel): void {
    this.restaurantSysAdminService.enableSupportForRestaurantAsync(restaurant.id)
      .pipe(take(1))
      .subscribe(() => {
        restaurant.needsSupport = true;
      }, response => {
        alert(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

  onDisableSupport(restaurant: RestaurantModel): void {
    this.restaurantSysAdminService.disableSupportForRestaurantAsync(restaurant.id)
      .pipe(take(1))
      .subscribe(() => {
        restaurant.needsSupport = false;
      }, response => {
        alert(this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors());
      });
  }

}