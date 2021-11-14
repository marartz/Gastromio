import { Component, OnInit, Input } from '@angular/core';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { DishModel } from '../../../shared/models/dish.model';

import { RestaurantAdminFacade } from '../../restaurant-admin.facade';

@Component({
  selector: 'app-remove-dish',
  templateUrl: './remove-dish.component.html',
  styleUrls: [
    './remove-dish.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css',
  ],
})
export class RemoveDishComponent implements OnInit {
  @Input() public dishCategoryId: string;
  @Input() public dish: DishModel;
  @BlockUI() blockUI: NgBlockUI;

  constructor(
    public activeModal: NgbActiveModal,
    private facade: RestaurantAdminFacade
  ) {}

  ngOnInit() {}

  onSubmit() {
    this.facade.removeDish(this.dishCategoryId, this.dish.id).subscribe(() => {
      this.activeModal.close();
    });
  }
}
