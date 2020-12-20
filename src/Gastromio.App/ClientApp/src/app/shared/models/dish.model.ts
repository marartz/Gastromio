import {DishVariantModel} from './dish-variant.model';

export class DishModel {
  constructor(init?: Partial<DishModel>) {
    Object.assign(this, init);
    this.id = undefined;
  }

  public id: string;

  public name: string;

  public description: string;

  public productInfo: string;

  public variants: Array<DishVariantModel>;
}
