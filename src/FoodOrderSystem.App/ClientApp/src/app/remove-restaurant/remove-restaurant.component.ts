import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { RestaurantSysAdminService } from '../restaurant-sys-admin/restaurant-sys-admin.service';
import { RestaurantModel } from '../restaurant/restaurant.model';
import { BlockUI, NgBlockUI } from 'ng-block-ui';

@Component({
  selector: 'app-remove-restaurant',
  templateUrl: './remove-restaurant.component.html',
  styleUrls: ['./remove-restaurant.component.css', '../../assets/css/admin-forms.min.css']
})
export class RemoveRestaurantComponent implements OnInit {
  @Input() public restaurant: RestaurantModel;
  @BlockUI() blockUI: NgBlockUI;

  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private restaurantAdminService: RestaurantSysAdminService,
  ) {
  }

  ngOnInit() {
  }

  onSubmit() {
    this.blockUI.start("Verarbeite Daten...");
    let subscription = this.restaurantAdminService.removeRestaurantAsync(this.restaurant.id)
      .subscribe(() => {
        subscription.unsubscribe();
        this.blockUI.stop();
        this.message = undefined;
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
