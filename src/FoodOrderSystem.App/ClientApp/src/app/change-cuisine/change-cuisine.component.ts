import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { CuisineModel } from '../cuisine/cuisine.model';
import { CuisineAdminService } from '../cuisine/cuisine-admin.service';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-change-cuisine',
  templateUrl: './change-cuisine.component.html',
  styleUrls: ['./change-cuisine.component.css', '../../assets/css/admin-forms.min.css']
})
export class ChangeCuisineComponent implements OnInit {
  @Input() public cuisine: CuisineModel;
  @BlockUI() blockUI: NgBlockUI;

  changeCuisineForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private cuisineAdminService: CuisineAdminService,
  ) {
  }

  ngOnInit() {
    this.changeCuisineForm = this.formBuilder.group({
      name: this.cuisine.name
    });
  }

  onSubmit(data) {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.cuisineAdminService.changeCuisineAsync(this.cuisine.id, data.name)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
        this.changeCuisineForm.reset();
        this.activeModal.close('Close click');
      }, (status: number) => {
        subscription.unsubscribe();
        this.blockUI.stop();
        if (status === 401)
          this.message = "Sie sind nicht angemdeldet.";
        else if (status === 403)
          this.message = "Sie sind nicht berechtigt, diese Aktion durchzuführen.";
        else
          this.message = "Ein unvorhergesehener Fehler ist aufgetreten. Bitte versuchen Sie es nochmals.";
      });
  }
}
