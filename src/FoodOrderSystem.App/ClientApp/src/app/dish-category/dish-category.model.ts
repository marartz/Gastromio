import { DishModel } from './dish.model';

export class DishCategoryModel {
  constructor(init?: Partial<DishCategoryModel>) {
    Object.assign(this, init);
  }

  public id: string;

  public name: string;

  public dishes: Array<DishModel>;
}
