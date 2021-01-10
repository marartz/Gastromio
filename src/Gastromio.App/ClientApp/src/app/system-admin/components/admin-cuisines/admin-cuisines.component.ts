import {Component, OnInit} from '@angular/core';

import {Observable} from 'rxjs';

import {NgbModal} from '@ng-bootstrap/ng-bootstrap';

import {CuisineModel} from '../../../shared/models/cuisine.model';

import {CuisineAdminService} from '../../services/cuisine-admin.service';

import {AddCuisineComponent} from '../add-cuisine/add-cuisine.component';
import {ChangeCuisineComponent} from '../change-cuisine/change-cuisine.component';
import {RemoveCuisineComponent} from '../remove-cuisine/remove-cuisine.component';

@Component({
  selector: 'app-admin-cuisines',
  templateUrl: './admin-cuisines.component.html',
  styleUrls: [
    './admin-cuisines.component.css',
    '../../../../assets/css/frontend_v3.min.css',
    '../../../../assets/css/backend_v2.min.css'
  ]
})
export class AdminCuisinesComponent implements OnInit {
  cuisines: Observable<CuisineModel[]>;

  constructor(
    private modalService: NgbModal,
    private cuisineAdminService: CuisineAdminService
  ) {
  }

  ngOnInit() {
    this.updateSearch();
  }

  openAddCuisineForm(): void {
    const modalRef = this.modalService.open(AddCuisineComponent);
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => {
    });
  }

  openChangeCuisineForm(cuisine: CuisineModel): void {
    const modalRef = this.modalService.open(ChangeCuisineComponent);
    modalRef.componentInstance.cuisine = cuisine;
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => {
    });
  }

  openRemoveCuisineForm(cuisine: CuisineModel): void {
    const modalRef = this.modalService.open(RemoveCuisineComponent);
    modalRef.componentInstance.cuisine = cuisine;
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => {
    });
  }

  updateSearch(): void {
    this.cuisines = this.cuisineAdminService.getAllCuisinesAsync();
  }
}