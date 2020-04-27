import { Component, OnInit, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';
import { RestaurantRestAdminService } from '../restaurant-rest-admin/restaurant-rest-admin.service';
import { RestaurantModel } from '../restaurant/restaurant.model';

@Component({
  selector: 'app-change-restaurant-name',
  templateUrl: './change-restaurant-name.component.html',
  styleUrls: ['./change-restaurant-name.component.css']
})
export class ChangeRestaurantNameComponent implements OnInit {
  @Input() public restaurant: RestaurantModel;

  changeRestaurantNameForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private restaurantAdminService: RestaurantRestAdminService,
  ) {
  }

  ngOnInit() {
    this.changeRestaurantNameForm = this.formBuilder.group({
      name: this.restaurant.name
    });
  }

  onSubmit(data) {
    this.restaurantAdminService.changeRestaurantNameAsync(this.restaurant.id, data.name)
      .subscribe(() => {
        this.message = undefined;
        this.changeRestaurantNameForm.reset();
        this.activeModal.close(data.name);
      }, (status: number) => {
          if (status === 401)
            this.message = "Sie sind nicht angemdeldet.";
          else if (status === 403)
            this.message = "Sie sind nicht berechtigt, diese Aktion durchzuführen.";
          else
            this.message = "Ein unvorhergesehener Fehler ist aufgetreten. Bitte versuchen Sie es nochmals.";
      });
  }
}
