import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { RestaurantSysAdminService } from '../restaurant-sys-admin/restaurant-sys-admin.service';

@Component({
  selector: 'app-add-restaurant',
  templateUrl: './add-restaurant.component.html',
  styleUrls: ['./add-restaurant.component.css']
})
export class AddRestaurantComponent implements OnInit {
  addRestaurantForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private restaurantAdminService: RestaurantSysAdminService,
  ) {
    this.addRestaurantForm = this.formBuilder.group({
      name: ''
    });
  }

  ngOnInit() {
  }

  onSubmit(data) {
    this.restaurantAdminService.addRestaurantAsync(data.name)
      .subscribe(() => {
        this.message = undefined;
        this.addRestaurantForm.reset();
        this.activeModal.close('Close click');
      }, (status: number) => {
          this.addRestaurantForm.reset();
          if (status === 401)
            this.message = "Sie sind nicht angemdeldet.";
          else if (status === 403)
            this.message = "Sie sind nicht berechtigt, diese Aktion durchzuführen.";
          else
            this.message = "Ein unvorhergesehener Fehler ist aufgetreten. Bitte versuchen Sie es nochmals.";
      });
  }
}
