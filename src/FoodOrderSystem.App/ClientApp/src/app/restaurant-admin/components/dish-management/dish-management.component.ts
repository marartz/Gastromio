import {Component, OnDestroy, OnInit} from '@angular/core';

import {BehaviorSubject, Subscription} from "rxjs";

import {NgbModal} from "@ng-bootstrap/ng-bootstrap";

import {DishCategoryModel} from "../../../shared/models/dish-category.model";
import {DishModel} from "../../../shared/models/dish.model";

import {RestaurantAdminFacade} from "../../restaurant-admin.facade";

import {AddDishCategoryComponent} from "../add-dish-category/add-dish-category.component";
import {RemoveDishCategoryComponent} from "../remove-dish-category/remove-dish-category.component";
import {ChangeDishCategoryComponent} from "../change-dish-category/change-dish-category.component";

@Component({
  selector: 'app-dish-management',
  templateUrl: './dish-management.component.html',
  styleUrls: [
    './dish-management.component.css',
    '../../../../assets/css/frontend_v2.min.css',
    '../../../../assets/css/backend_v2.min.css'
  ]
})
export class DishManagementComponent implements OnInit, OnDestroy {

  public dishCategories: DishCategoryModel[];

  public activeDishCategoryId$: BehaviorSubject<string> = new BehaviorSubject<string>(undefined);

  private restaurantSubscription: Subscription;
  private dishCategoriesSubscription: Subscription;

  constructor(
    private facade: RestaurantAdminFacade,
    private modalService: NgbModal
  ) {
  }

  ngOnInit(): void {
    this.dishCategoriesSubscription = this.facade.getDishCategories$().subscribe(
      dishCategories => {
        this.dishCategories = dishCategories;

        if (this.activeDishCategoryId$.value === undefined) {
          this.activeDishCategoryId$.next(dishCategories.length > 0 ? dishCategories[0].id : undefined);
        }
        const index = dishCategories.findIndex(en => en.id === this.activeDishCategoryId$.value)
        if (index < 0) {
          this.activeDishCategoryId$.next(dishCategories.length > 0 ? dishCategories[0].id : undefined);
        }
      }
    );
  }

  ngOnDestroy(): void {
    this.restaurantSubscription?.unsubscribe();
    this.dishCategoriesSubscription?.unsubscribe();
  }


  public isFirstDishCategory(dishCategory: DishCategoryModel): boolean {
    return this.facade.isFirstDishCategory(dishCategory);
  }

  public isLastDishCategory(dishCategory: DishCategoryModel): boolean {
    return this.facade.isLastDishCategory(dishCategory);
  }

  public isFirstDish(dishCategory: DishCategoryModel, dish: DishModel): boolean {
    return false;
  }

  public isLastDish(dishCategory: DishCategoryModel, dish: DishModel): boolean {
    return false;
  }

  public getPricesOfDish(dish: DishModel): string {
    return "";
  }


  public onChangeActiveDishCategoryId(dishCategoryId: string): void {
    this.activeDishCategoryId$.next(dishCategoryId);
  }

  public openAddDishCategoryForm(): void {
    const modalRef = this.modalService.open(AddDishCategoryComponent);
    if (this.dishCategories !== undefined && this.dishCategories.length > 0) {
      modalRef.componentInstance.afterCategoryId = this.dishCategories[this.dishCategories.length - 1].id;
    }
    modalRef.result.then(id => {
      this.activeDishCategoryId$.next(id);
    }, () => {});
  }

  public openChangeDishCategoryForm(dishCategory: DishCategoryModel): void {
    const modalRef = this.modalService.open(ChangeDishCategoryComponent);
    modalRef.componentInstance.dishCategory = dishCategory;
    modalRef.result.then(() => {}, () => {});
  }

  public openRemoveDishCategoryForm(dishCategory: DishCategoryModel): void {
    const modalRef = this.modalService.open(RemoveDishCategoryComponent);
    modalRef.componentInstance.dishCategory = dishCategory;
    modalRef.result.then(() => {}, () => {});
  }

  public decOrderOfDishCategory(dishCategory: DishCategoryModel): void {
    this.facade.decOrderOfDishCategory(dishCategory);
  }

  public incOrderOfDishCategory(dishCategory: DishCategoryModel): void {
    this.facade.incOrderOfDishCategory(dishCategory);
  }

  public openEditDishForm(dishCategory: DishCategoryModel, dish: DishModel): void {

  }

  public openRemoveDishForm(dishCategory: DishCategoryModel, dish: DishModel): void {

  }

  public decOrderOfDish(dishCategory: DishCategoryModel, dish: DishModel): void {

  }

  public incOrderOfDish(dishCategory: DishCategoryModel, dish: DishModel): void {

  }

}
