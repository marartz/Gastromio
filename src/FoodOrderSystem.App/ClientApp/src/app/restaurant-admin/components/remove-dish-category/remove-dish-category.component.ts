import {Component, OnInit, Input} from '@angular/core';

import {take} from 'rxjs/operators';

import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {BlockUI, NgBlockUI} from 'ng-block-ui';

import {DishCategoryModel} from '../../../shared/models/dish-category.model';

import {RestaurantAdminFacade} from "../../restaurant-admin.facade";

@Component({
  selector: 'app-remove-dish-category',
  templateUrl: './remove-dish-category.component.html',
  styleUrls: [
    './remove-dish-category.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css'
  ]
})
export class RemoveDishCategoryComponent implements OnInit {
  @Input() public dishCategory: DishCategoryModel;
  @BlockUI() blockUI: NgBlockUI;

  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    public facade: RestaurantAdminFacade
  ) {
  }

  ngOnInit() {
  }

  onSubmit() {
    this.facade.removeDishCategory(this.dishCategory.id)
      .pipe(take(1))
      .subscribe(() => {
        this.activeModal.close();
      });
  }
}
