import {Component, OnInit} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';

import {take} from 'rxjs/operators';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {HttpErrorHandlingService} from '../../../shared/services/http-error-handling.service';

import {ImportLogLineModel} from '../../models/import-log-line.model';

import {RestaurantSysAdminService} from '../../services/restaurant-sys-admin.service';

@Component({
  selector: 'app-admin-dish-import',
  templateUrl: './admin-dish-import.component.html',
  styleUrls: ['./admin-dish-import.component.css', '../../../../assets/css/frontend_v2.min.css', '../../../../assets/css/backend_v2.min.css']
})
export class AdminDishImportComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;

  error: string;

  dishImportFile: File;

  logLines: Array<ImportLogLineModel>;

  constructor(
    private restaurantSysAdminService: RestaurantSysAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
  }

  handleDishImportFileInput(target: any): void {
    const files = target.files;
    this.dishImportFile = files.item(0);
  }

  onSimulateDishes(): void {
    this.error = undefined;
    this.logLines = undefined;

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantSysAdminService.importDishesAsync(this.dishImportFile, true)
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

  onImportDishes(): void {
    this.error = undefined;
    this.logLines = undefined;

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantSysAdminService.importDishesAsync(this.dishImportFile, false)
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
