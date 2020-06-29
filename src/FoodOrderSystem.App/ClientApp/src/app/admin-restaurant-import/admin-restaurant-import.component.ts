import {Component, OnInit} from '@angular/core';
import {RestaurantSysAdminService} from '../restaurant-sys-admin/restaurant-sys-admin.service';
import {take} from 'rxjs/operators';
import {HttpErrorResponse} from '@angular/common/http';
import {BlockUI, NgBlockUI} from 'ng-block-ui';
import {HttpErrorHandlingService} from '../http-error-handling/http-error-handling.service';
import {ImportLogLineModel} from '../restaurant-sys-admin/import-log-line.model';

@Component({
  selector: 'app-admin-restaurant-import',
  templateUrl: './admin-restaurant-import.component.html',
  styleUrls: ['./admin-restaurant-import.component.css', '../../assets/css/frontend.min.css', '../../assets/css/backend.min.css']
})
export class AdminRestaurantImportComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;

  error: string;

  restaurantImportFile: File;

  logLines: Array<ImportLogLineModel>;

  constructor(
    private restaurantSysAdminService: RestaurantSysAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
  }

  handleRestaurantImportFileInput(target: any): void {
    const files = target.files;
    console.log('files: ', files);
    this.restaurantImportFile = files.item(0);
  }

  onSimulateRestaurants(): void {
    this.error = undefined;
    this.logLines = undefined;

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantSysAdminService.importRestaurantsAsync(this.restaurantImportFile, true)
      .pipe(take(1))
      .subscribe((log) => {
        this.blockUI.stop();
        this.error = undefined;
        this.logLines = log.lines;
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.error = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
        this.logLines = undefined;
      });
  }

  onImportRestaurants(): void {
    this.error = undefined;
    this.logLines = undefined;

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantSysAdminService.importRestaurantsAsync(this.restaurantImportFile, false)
      .pipe(take(1))
      .subscribe((log) => {
        this.blockUI.stop();
        this.error = undefined;
        this.logLines = log.lines;
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.error = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
        this.logLines = undefined;
      });
  }

  getColorForLogLineType(type: number): string {
    switch (type)
    {
      case 0:
        return 'green';
      case 1:
        return 'yellow';
      case 2:
        return 'red';
      default:
        return '';
    }
  }
}
