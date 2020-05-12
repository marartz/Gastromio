import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CuisineAdminService } from '../cuisine/cuisine-admin.service';
import { AddCuisineComponent } from '../add-cuisine/add-cuisine.component';
import { CuisineModel } from '../cuisine/cuisine.model';
import { Observable } from 'rxjs';
import { ChangeCuisineComponent } from '../change-cuisine/change-cuisine.component';
import { RemoveCuisineComponent } from '../remove-cuisine/remove-cuisine.component';

@Component({
  selector: 'app-admin-cuisines',
  templateUrl: './admin-cuisines.component.html',
  styleUrls: ['./admin-cuisines.component.css', '../../assets/css/admin.min.css']
})
export class AdminCuisinesComponent implements OnInit {
  cuisines: Observable<CuisineModel[]>;

  constructor(
    private modalService: NgbModal,
    private cuisineAdminService: CuisineAdminService
  ) { }

  ngOnInit() {
    this.updateSearch();
  }

  openAddCuisineForm(): void {
    let modalRef = this.modalService.open(AddCuisineComponent);
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => { });
  }

  openChangeCuisineForm(cuisine: CuisineModel): void {
    let modalRef = this.modalService.open(ChangeCuisineComponent);
    modalRef.componentInstance.cuisine = cuisine;
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => { });
  }

  openRemoveCuisineForm(cuisine: CuisineModel): void {
    let modalRef = this.modalService.open(RemoveCuisineComponent);
    modalRef.componentInstance.cuisine = cuisine;
    modalRef.result.then(() => {
      this.updateSearch();
    }, () => { });
  }

  updateSearch(): void {
    this.cuisines = this.cuisineAdminService.getAllCuisinesAsync();
  }
}
