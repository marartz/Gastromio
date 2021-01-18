import {Injectable} from "@angular/core";

import {BehaviorSubject, Observable} from "rxjs";

import {HttpErrorHandlingService} from "../shared/services/http-error-handling.service";

import {RestaurantSysAdminService} from "./services/restaurant-sys-admin.service";
import {Router} from "@angular/router";

@Injectable()
export class SystemAdminFacade {

  private isInitializing$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
  private isInitialized$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(undefined);
  private initializationError$: BehaviorSubject<string> = new BehaviorSubject<string>(undefined);

  private selectedTab$: BehaviorSubject<string> = new BehaviorSubject<string>("general");

  private isUpdating$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  private isUpdated$: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(undefined);
  private updateError$: BehaviorSubject<string> = new BehaviorSubject<string>(undefined);

  static earliestOpeningTime: number = 4 * 60;

  constructor(
    private sysAdminService: RestaurantSysAdminService,
    private router: Router,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  public initialize(tab: string): void {
    this.selectedTab$.next(tab);
    this.isInitializing$.next(false);
    this.isInitialized$.next(true);
  }

  public getIsInitializing$(): Observable<boolean> {
    return this.isInitializing$;
  }

  public getIsInitialized$(): Observable<boolean> {
    return this.isInitialized$;
  }

  public getInitializationError$(): Observable<string> {
    return this.initializationError$;
  }

  public getSelectedTab$(): Observable<string> {
    return this.selectedTab$;
  }

  public getIsUpdating$(): Observable<boolean> {
    return this.isUpdating$;
  }

  public getIsUpdated$(): Observable<boolean> {
    return this.isUpdated$;
  }

  public getUpdateError$(): Observable<string> {
    return this.updateError$;
  }


  // actions

  public selectTab(tab: string): void {
    this.router.navigate(['admin', tab], );
  }

}
