import { Component, OnInit, Input } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';
import { DishCategoryModel } from '../dish-category/dish-category.model';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DishModel } from '../dish-category/dish.model';
import { HttpErrorHandlingService } from '../http-error-handling/http-error-handling.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-remove-dish',
  templateUrl: './remove-dish.component.html',
  styleUrls: ['./remove-dish.component.css', '../../assets/css/admin-forms.min.css']
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
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantAdminService.removeDishFromRestaurantAsync(this.restaurantId, this.dishCategoryId, this.dish.id)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.activeModal.close('Close click');
      }, (response: HttpErrorResponse) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = this.httpErrorHandlingService.handleError(response).getJoinedGeneralErrors();
      });
  }
}
