import { Component, OnInit, Input } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';

import { Observable } from 'rxjs';

import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { BlockUI, NgBlockUI } from 'ng-block-ui';

import { CuisineModel } from '../../../shared/models/cuisine.model';

import { SystemAdminFacade } from '../../system-admin.facade';

@Component({
  selector: 'app-change-cuisine',
  templateUrl: './change-cuisine.component.html',
  styleUrls: [
    './change-cuisine.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/modals.component.min.css',
    '../../../../assets/css/overlays/modals.min.css',
    '../../../../assets/css/application-ui/forms/input-groups.min.css',
  ],
})
export class ChangeCuisineComponent implements OnInit {
  @Input() public cuisine: CuisineModel;
  @BlockUI() blockUI: NgBlockUI;

  changeCuisineForm: UntypedFormGroup;
  message$: Observable<string>;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: UntypedFormBuilder,
    private facade: SystemAdminFacade
  ) {}

  ngOnInit() {
    this.facade.getIsUpdating$().subscribe((isUpdating) => {
      if (isUpdating) {
        this.blockUI.start('Verarbeite Daten...');
      } else {
        this.blockUI.stop();
      }
    });

    this.message$ = this.facade.getUpdateError$();

    this.changeCuisineForm = this.formBuilder.group({
      name: [this.cuisine.name, Validators.required],
    });
  }

  get f() {
    return this.changeCuisineForm.controls;
  }

  onSubmit(data) {
    if (this.changeCuisineForm.invalid) {
      return;
    }

    this.facade.changeCuisine$(this.cuisine.id, data.name).subscribe(() => {
      this.activeModal.close('Close click');
    });
  }
}
