import {Component, OnInit, OnDestroy} from '@angular/core';
import {Subject} from 'rxjs';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {debounceTime, distinctUntilChanged, take} from 'rxjs/operators';

import {RestaurantSysAdminService} from '../restaurant-sys-admin/restaurant-sys-admin.service';
import {AddRestaurantComponent} from '../add-restaurant/add-restaurant.component';
import {RemoveRestaurantComponent} from '../remove-restaurant/remove-restaurant.component';
import {RestaurantModel} from '../restaurant/restaurant.model';
import {ChangePageInfo} from '../pagination/server-pagination.component';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {RestaurantRestAdminService} from '../restaurant-rest-admin/restaurant-rest-admin.service';

@Component({
  selector: 'app-admin-restaurants',
  templateUrl: './admin-restaurants.component.html',
  styleUrls: ['./admin-restaurants.component.css', '../../assets/css/frontend.min.css', '../../assets/css/backend.min.css']
})
export class AdminRestaurantsComponent implements OnInit, OnDestroy {
  total: number;
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
        this.updateSearch();
      });
  }

  ngOnInit() {
    this.searchPhrase = '';
    this.updateSearch();
  }

  ngOnDestroy() {
  }

  openAddRestaurantForm(): void {
    const modalRef = this.modalService.open(AddRestaurantComponent);
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => {
    });
  }

  openRemoveRestaurantForm(restaurant: RestaurantModel): void {
    const modalRef = this.modalService.open(RemoveRestaurantComponent);
    modalRef.componentInstance.restaurant = restaurant;
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => {
    });
  }

  onSearchType(value: string) {
    this.searchPhraseUpdated.next(value);
  }

  onChangePage(changePageInfo: ChangePageInfo) {
    this.restaurantSysAdminService.searchForRestaurantsAsync(this.searchPhrase, changePageInfo.skip, changePageInfo.take)
      .pipe(take(1))
      .subscribe((result) => {
        this.total = result.total;
        this.pageOfRestaurants = result.items;
      }, () => {
      });
  }

  updateSearch(): void {
    this.restaurantSysAdminService.searchForRestaurantsAsync(this.searchPhrase, 0, 0)
      .pipe(take(1))
      .subscribe((result) => {
        this.total = result.total;
        this.pageOfRestaurants = result.items;
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

}
