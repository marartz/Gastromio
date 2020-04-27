import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { RestaurantSysAdminService } from '../restaurant-sys-admin/restaurant-sys-admin.service';
import { RestaurantModel } from '../restaurant/restaurant.model';

@Component({
  selector: 'app-remove-restaurant',
  templateUrl: './remove-restaurant.component.html',
  styleUrls: ['./remove-restaurant.component.css']
})
export class RemoveRestaurantComponent implements OnInit {
  @Input() public restaurant: RestaurantModel;

  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private restaurantAdminService: RestaurantSysAdminService,
  ) {
  }

  ngOnInit() {
  }

  onSubmit() {
    this.restaurantAdminService.removeRestaurantAsync(this.restaurant.id)
      .subscribe(() => {
        this.message = undefined;
        this.activeModal.close('Close click');
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
