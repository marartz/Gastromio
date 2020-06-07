import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

import { RestaurantSysAdminService } from '../restaurant-sys-admin/restaurant-sys-admin.service';
import { AddRestaurantComponent } from '../add-restaurant/add-restaurant.component';
import { RemoveRestaurantComponent } from '../remove-restaurant/remove-restaurant.component';
import { RestaurantModel } from '../restaurant/restaurant.model';
import {ChangePageInfo} from '../pagination/server-pagination.component';

@Component({
  selector: 'app-admin-restaurants',
  templateUrl: './admin-restaurants.component.html',
  styleUrls: ['./admin-restaurants.component.css', '../../assets/css/admin.min.css']
})
export class AdminRestaurantsComponent implements OnInit, OnDestroy {
  total: number;
  pageOfRestaurants: RestaurantModel[];

  private searchPhrase: string;
  private searchPhraseUpdated: Subject<string> = new Subject<string>();

  private updateSearchSubscription: Subscription;

  constructor(
    private modalService: NgbModal,
    private restaurantAdminService: RestaurantSysAdminService
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

  openAddRestaurantForm(): void {
    const modalRef = this.modalService.open(AddRestaurantComponent);
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => { });
  }

  openRemoveRestaurantForm(restaurant: RestaurantModel): void {
    const modalRef = this.modalService.open(RemoveRestaurantComponent);
    modalRef.componentInstance.restaurant = restaurant;
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => { });
  }

  onSearchType(value: string) {
    this.searchPhraseUpdated.next(value);
  }

  onChangePage(changePageInfo: ChangePageInfo) {
    const observable = this.restaurantAdminService.searchForRestaurantsAsync(this.searchPhrase, changePageInfo.skip, changePageInfo.take);

    const subscription = observable.subscribe((result) => {
      subscription.unsubscribe();
      this.total = result.total;
      this.pageOfRestaurants = result.items;
    }, () => {
      subscription.unsubscribe();
    });
  }

  updateSearch(): void {
    if (this.updateSearchSubscription !== undefined) {
      this.updateSearchSubscription.unsubscribe();
    }

    const observable = this.restaurantAdminService.searchForRestaurantsAsync(this.searchPhrase, 0, 0);

    this.updateSearchSubscription = observable.subscribe((result) => {
      this.total = result.total;
      this.pageOfRestaurants = result.items;
    }, () => {
    });
  }
}
