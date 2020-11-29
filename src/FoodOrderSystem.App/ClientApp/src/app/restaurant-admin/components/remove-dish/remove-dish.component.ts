import {Component, OnInit, Input} from '@angular/core';
import {HttpErrorResponse} from '@angular/common/http';

import {take} from 'rxjs/operators';

import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {DishModel} from '../../../shared/models/dish.model';

import {HttpErrorHandlingService} from '../../../shared/services/http-error-handling.service';

import {RestaurantRestAdminService} from '../../services/restaurant-rest-admin.service';

@Component({
  selector: 'app-remove-dish',
  templateUrl: './remove-dish.component.html',
  styleUrls: [
    './remove-dish.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class RemoveDishComponent implements OnInit {
  @Input() public restaurantId: string;
  @Input() public dishCategoryId: string;
  @Input() public dish: DishModel;
  @BlockUI() blockUI: NgBlockUI;

  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private restaurantAdminService: RestaurantRestAdminService,
    private httpErrorHandlingService: HttpErrorHandlingService
  ) {
  }

  ngOnInit() {
  }

  onSubmit() {
    this.blockUI.start('Verarbeite Daten...');
    this.restaurantAdminService.removeDishFromRestaurantAsync(this.restaurantId, this.dishCategoryId, this.dish.id)
      .pipe(take(1))
      .subscribe(() => {
        this.blockUI.stop();
        this.message = undefined;
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
