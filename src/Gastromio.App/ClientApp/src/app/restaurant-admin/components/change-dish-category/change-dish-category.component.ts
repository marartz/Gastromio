import { Component, OnInit, Input } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';

import { Observable } from 'rxjs';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { DishCategoryModel } from '../../../shared/models/dish-category.model';

import { RestaurantAdminFacade } from '../../restaurant-admin.facade';

@Component({
  selector: 'app-change-dish-category',
  templateUrl: './change-dish-category.component.html',
  styleUrls: [
    './change-dish-category.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css',
  ],
})
export class ChangeDishCategoryComponent implements OnInit {
  @Input() public dishCategory: DishCategoryModel;
  @BlockUI() blockUI: NgBlockUI;

  changeDishCategoryForm: UntypedFormGroup;
  message$: Observable<string>;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: UntypedFormBuilder,
    private facade: RestaurantAdminFacade
  ) {}

  ngOnInit() {
    this.message$ = this.facade.getUpdateError$();

    this.changeDishCategoryForm = this.formBuilder.group({
      name: [this.dishCategory.name, Validators.required],
    });
  }

  get f() {
    return this.changeDishCategoryForm.controls;
  }

  onSubmit(data) {
    if (this.changeDishCategoryForm.invalid) {
      return;
    }

    this.facade
      .changeDishCategory(this.dishCategory.id, data.name)
      .subscribe(() => {
        this.changeDishCategoryForm.reset();
        this.activeModal.close();
      });
  }
}
