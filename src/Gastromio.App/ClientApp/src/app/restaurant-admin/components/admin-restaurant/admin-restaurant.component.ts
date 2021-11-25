import {Component, OnInit, OnDestroy} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {concat, merge, Observable, of, Subscription} from 'rxjs';
import {delay, switchMap} from 'rxjs/operators';
import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {RestaurantModel} from '../../../shared/models/restaurant.model';

import {RestaurantAdminFacade} from '../../restaurant-admin.facade';
import {LinkInfo} from '../../../shared/components/scrollable-nav-bar/scrollable-nav-bar.component';

@Component({
  selector: 'app-admin-restaurant',
  templateUrl: './admin-restaurant.component.html',
  styleUrls: [
    './admin-restaurant.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/backend_v2.min.css',
    '../../../../assets/css/application-ui/overlays/notifications.min.css',
    '../../../../assets/css/marketing/page-sections/error-page.min.css',
  ],
})
export class AdminRestaurantComponent implements OnInit, OnDestroy {
  @BlockUI() blockUI: NgBlockUI;

  public isInitialized$: Observable<boolean>;
  public initializationError$: Observable<string>;
  public restaurant$: Observable<RestaurantModel>;
  public selectedTab$: Observable<string>;
  public isUpdated$: Observable<boolean>;
  public updateError: string;

  private updateErrorSubscription: Subscription;

  public links: Array<LinkInfo> = [
    {id: 'general', name: 'Allgemein'},
    {id: 'order', name: 'Bestellung'},
    {id: 'payment', name: 'Zahlung'},
    {id: 'opening-hours', name: 'Zeiten'},
    {id: 'dishes', name: 'Speisen'},
  ];

  constructor(
    private route: ActivatedRoute,
    private facade: RestaurantAdminFacade
  ) {
  }

  ngOnInit() {
    this.facade.getIsInitializing$().subscribe((isInitializing) => {
      if (isInitializing) {
        this.blockUI.start('Lade Restaurantdaten...');
      } else {
        this.blockUI.stop();
      }
    });

    this.facade.getIsUpdating$().subscribe((isUpdating) => {
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

    this.isUpdated$ = this.facade.getIsUpdated$().pipe(
      switchMap((isUpdated) => {
        if (isUpdated) {
          return merge(of(true), of(false).pipe(delay(2000)));
        } else {
          return of(false);
        }
      })
    );

    this.updateErrorSubscription = concat(
      of(undefined),
      this.facade.getUpdateError$()
        .pipe(switchMap((message) => {
            console.log("message: ", message);
            return concat(of(message), of(undefined).pipe(delay(5000)));
          })
        )
    ).subscribe((message) => {
      this.updateError = message;
    });

    this.route.paramMap.subscribe((params) => {
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
