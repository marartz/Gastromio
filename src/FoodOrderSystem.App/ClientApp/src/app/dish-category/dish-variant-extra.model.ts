import { DishVariantModel } from './dish-variant.model';

export class DishVariantExtraModel {
  constructor(init?: Partial<DishVariantExtraModel>) {
    Object.assign(this, init);
  }

  public name: string;

  public productInfo: string;

  public price: number;
}
