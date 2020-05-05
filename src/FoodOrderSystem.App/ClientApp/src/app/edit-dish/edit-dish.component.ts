import { Component, OnInit, Input } from '@angular/core';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { FormGroup, FormBuilder } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DishModel } from '../dish-category/dish.model';

@Component({
  selector: 'app-edit-dish',
  templateUrl: './edit-dish.component.html',
  styleUrls: ['./edit-dish.component.css']
})
export class EditDishComponent implements OnInit {
  @Input() public restaurantId: string;
  @Input() public dishCategoryId: string;
  @Input() public dish: DishModel;
  @BlockUI() blockUI: NgBlockUI;

  isNew: boolean;
  editDishForm: FormGroup;
  message: string;

  constructor(
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder
  ) {
    this.editDishForm = this.formBuilder.group({
      name: '',
      description: '',
      productInfo: '',
    });
  }

  ngOnInit() {
    this.isNew = this.dish === undefined || this.dish.id === undefined;
  }

  onSubmit(data) {
    //this.blockUI.start("Verarbeite Daten...");
    //let subscription = this.cuisineAdminService.addCuisineAsync(data.name)
    //  .subscribe(() => {
    //    subscription.unsubscribe();
    //    this.blockUI.stop();
    //    this.message = undefined;
    //    this.addCuisineForm.reset();
    //    this.activeModal.close('Close click');
    //  }, (status: number) => {
    //    subscription.unsubscribe();
    //    this.blockUI.stop();
    //    this.addCuisineForm.reset();
    //    if (status === 401)
    //      this.message = "Sie sind nicht angemdeldet.";
    //    else if (status === 403)
    //      this.message = "Sie sind nicht berechtigt, diese Aktion durchzuführen.";
    //    else
    //      this.message = "Ein unvorhergesehener Fehler ist aufgetreten. Bitte versuchen Sie es nochmals.";
    //  });
  }
}
