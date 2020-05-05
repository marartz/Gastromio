import { DishVariantExtraModel } from './dish-variant-extra.model';

export class DishVariantModel {
  constructor(init?: Partial<DishVariantModel>) {
    Object.assign(this, init);
  }

  public name: string;

  public price: number;

  public extras: Array<DishVariantExtraModel>;
}
