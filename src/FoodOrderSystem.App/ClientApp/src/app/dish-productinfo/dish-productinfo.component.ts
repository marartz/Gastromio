import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { DishModel } from '../dish-category/dish.model';

@Component({
  selector: 'app-dish-productinfo',
  templateUrl: './dish-productinfo.component.html',
  styleUrls: ['./dish-productinfo.component.css']
})
export class DishProductInfoComponent implements OnInit {
  @Input() dish: DishModel;

  constructor(
    public activeModal: NgbActiveModal
  ) {
  }

  ngOnInit() {
  }
}
