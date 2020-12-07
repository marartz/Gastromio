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
    const pos = this.dishCategories.findIndex(en => en.id === dishCategory.id);
    return pos === 0;
  }

  public isLastDishCategory(dishCategory: DishCategoryModel): boolean {
    const pos = this.dishCategories.findIndex(en => en.id === dishCategory.id);
    return pos === this.dishCategories.length - 1;
  }

  public isFirstDish(dishCategory: DishCategoryModel, dish: DishModel): boolean {
    const pos = dishCategory.dishes.findIndex(en => en.id === dish.id);
    return pos === 0;
  }

  public isLastDish(dishCategory: DishCategoryModel, dish: DishModel): boolean {
    const pos = dishCategory.dishes.findIndex(en => en.id === dish.id);
    return pos === dishCategory.dishes.length - 1;
  }

  public getPricesOfDish(dish: DishModel): string {
    if (!dish.variants || dish.variants.length === 0) {
      return '';
    }

    if (dish.variants.length === 1) {
      return '€' + dish.variants[0].price.toLocaleString('de', {minimumFractionDigits: 2});
    }

    let result = '';
    for (const variant of dish.variants) {
      if (result.length > 0) {
        result += '; ';
      }
      result += variant.name + ' €' + variant.price.toLocaleString('de', {minimumFractionDigits: 2});
    }

    return result;
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
    this.facade.decOrderOfDishCategory(dishCategory.id);
  }

  public incOrderOfDishCategory(dishCategory: DishCategoryModel): void {
    this.facade.incOrderOfDishCategory(dishCategory.id);
  }

  public openEditDishForm(dishCategory: DishCategoryModel, dish: DishModel): void {

  }

  public openRemoveDishForm(dishCategory: DishCategoryModel, dish: DishModel): void {

  }

  public decOrderOfDish(dishCategory: DishCategoryModel, dish: DishModel): void {
    this.facade.decOrderOfDish(dishCategory.id, dish.id);
  }

  public incOrderOfDish(dishCategory: DishCategoryModel, dish: DishModel): void {
    this.facade.incOrderOfDish(dishCategory.id, dish.id);
  }

}
