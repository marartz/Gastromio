import {Component, OnInit, OnDestroy} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {RestaurantAdminFacade} from "../../restaurant-admin.facade";
import {Observable} from "rxjs";
import {RestaurantModel} from "../../../shared/models/restaurant.model";

@Component({
  selector: 'app-admin-restaurant',
  templateUrl: './admin-restaurant.component.html',
  styleUrls: [
    './admin-restaurant.component.css',
    '../../../../assets/css/frontend_v2.min.css',
    '../../../../assets/css/backend_v2.min.css'
  ]
})
export class AdminRestaurantComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  public isInitialized$: Observable<boolean>;
  public initializationError$: Observable<string>;
  public restaurant$: Observable<RestaurantModel>;
  public selectedTab$: Observable<string>;
  public updateError$: Observable<string>;

  constructor(
    private route: ActivatedRoute,
    private facade: RestaurantAdminFacade
  ) {
  }

  ngOnInit() {
    this.facade.getIsInitializing$().subscribe(isInitializing => {
      if (isInitializing) {
        this.blockUI.start('Lade Restaurantdaten...');
      } else {
        this.blockUI.stop();
      }
    });

    this.facade.getIsUpdating$().subscribe(isUpdating => {
      if (isUpdating) {
        this.blockUI.start('Speichere Restaurantdaten...');
      } else {
        this.blockUI.stop();
      }
    });

    this.isInitialized$ = this.facade.getIsInitialized$();
    this.initializationError$ = this.facade.getInitializationError$();
    this.restaurant$ = this.facade.getRestaurant$();
    this.selectedTab$ = this.facade.getSelectedTab$();
    this.updateError$ = this.facade.getUpdateError$();

    this.route.paramMap.subscribe(params => {
      const restaurantId = params.get('restaurantId');
      this.facade.initialize(restaurantId);
    });
  }

  ngOnDestroy() {
  }

  selectTab(tab: string): void {
    this.facade.selectTab(tab);
  }

}
