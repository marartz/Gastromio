import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup } from '@angular/forms';

import { CuisineModel } from '../cuisine/cuisine.model';
import { CuisineAdminService } from '../cuisine/cuisine-admin.service';

@Component({
  selector: 'app-change-cuisine',
  templateUrl: './change-cuisine.component.html',
  styleUrls: ['./change-cuisine.component.css']
})
export class ChangeCuisineComponent implements OnInit {
  @Input() public cuisine: CuisineModel;

  changeCuisineForm: FormGroup;
  message: string;

  imgUrl: any;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private cuisineAdminService: CuisineAdminService,
  ) {
  }

  ngOnInit() {
    this.changeCuisineForm = this.formBuilder.group({
      name: this.cuisine.name,
      image: this.cuisine.image
    });
    this.imgUrl = this.cuisine.image;
  }

  onImageChange(event) {
    if (!event.target.files || !event.target.files.length)
      return;
    let reader = new FileReader();
    const [file] = event.target.files;
    reader.readAsDataURL(file);

    reader.onload = () => {
      this.changeCuisineForm.patchValue({
        image: reader.result
      });

      this.imgUrl = reader.result;
    };
  }

  onSubmit(data) {
    this.cuisineAdminService.changeCuisineAsync(this.cuisine.id, data.name, data.image)
      .subscribe(() => {
        this.message = undefined;
        this.changeCuisineForm.reset();
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
