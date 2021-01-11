import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {HttpErrorHandlingService} from '../../../shared/services/http-error-handling.service';

import {ImportLogLineModel} from '../../models/import-log-line.model';

import {RestaurantSysAdminService} from '../../services/restaurant-sys-admin.service';

@Component({
  selector: 'app-admin-restaurant-import',
  templateUrl: './admin-restaurant-import.component.html',
  styleUrls: ['./admin-restaurant-import.component.css', '../../../../assets/css/frontend_v3.min.css', '../../../../assets/css/backend_v2.min.css']
})
export class AdminRestaurantImportComponent implements OnInit {
  @BlockUI() blockUI: NgBlockUI;
  @ViewChild('fileInput') fileInputVariable: ElementRef;

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
    this.restaurantImportFile = files.item(0);
  }

  onSimulateRestaurants(): void {
    if (!this.restaurantImportFile) {
      this.error = "Bitte wähle erst eine Importdatei aus.";
      return;
    }

    this.error = undefined;
    this.logLines = undefined;

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantSysAdminService.importRestaurantsAsync(this.restaurantImportFile, true)
      .subscribe((log) => {
        this.blockUI.stop();
        this.error = undefined;
        this.logLines = log.lines;
        //this.restaurantImportFile = undefined;
        //this.fileInputVariable.nativeElement.value = "";
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.error = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
        this.logLines = undefined;
        //this.restaurantImportFile = undefined;
        //this.fileInputVariable.nativeElement.value = "";
      });
  }

  onImportRestaurants(): void {
    if (!this.restaurantImportFile) {
      this.error = "Bitte wähle erst eine Importdatei aus.";
      return;
    }

    this.error = undefined;
    this.logLines = undefined;

    this.blockUI.start('Verarbeite Daten...');
    this.restaurantSysAdminService.importRestaurantsAsync(this.restaurantImportFile, false)
      .subscribe((log) => {
        this.blockUI.stop();
        this.error = undefined;
        this.logLines = log.lines;
        //this.restaurantImportFile = undefined;
        //this.fileInputVariable.nativeElement.value = "";
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.error = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
        this.logLines = undefined;
        //this.restaurantImportFile = undefined;
        //this.fileInputVariable.nativeElement.value = "";
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
