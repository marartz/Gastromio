import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { CuisineAdminService } from '../cuisine/cuisine-admin.service';
import { CuisineModel } from '../cuisine/cuisine.model';

@Component({
  selector: 'app-remove-cuisine',
  templateUrl: './remove-cuisine.component.html',
  styleUrls: ['./remove-cuisine.component.css']
})
export class RemoveCuisineComponent implements OnInit {
  @Input() public cuisine: CuisineModel;

  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private cuisineAdminService: CuisineAdminService,
  ) {
  }

  ngOnInit() {
  }

  onSubmit() {
    this.cuisineAdminService.removeCuisineAsync(this.cuisine.id)
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
