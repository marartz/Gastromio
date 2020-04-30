import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { CuisineAdminService } from '../cuisine/cuisine-admin.service';

@Component({
  selector: 'app-add-cuisine',
  templateUrl: './add-cuisine.component.html',
  styleUrls: ['./add-cuisine.component.css']
})
export class AddCuisineComponent implements OnInit {
  addCuisineForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private cuisineAdminService: CuisineAdminService,
  ) {
    this.addCuisineForm = this.formBuilder.group({
      name: ''
    });
  }

  ngOnInit() {
  }

  onSubmit(data) {
    this.cuisineAdminService.addCuisineAsync(data.name)
      .subscribe(() => {
        this.message = undefined;
        this.addCuisineForm.reset();
        this.activeModal.close('Close click');
      }, (status: number) => {
        this.addCuisineForm.reset();
        if (status === 401)
          this.message = "Sie sind nicht angemdeldet.";
        else if (status === 403)
          this.message = "Sie sind nicht berechtigt, diese Aktion durchzuführen.";
        else
          this.message = "Ein unvorhergesehener Fehler ist aufgetreten. Bitte versuchen Sie es nochmals.";
      });
  }
}
