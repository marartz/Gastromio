import { DishVariantExtraModel } from './dish-variant-extra.model';

export class DishVariantModel {
  constructor(init?: Partial<DishVariantModel>) {
    if (init) {
      Object.assign(this, init);
    }
    this.extras = this.extras?.map((extra) => new DishVariantExtraModel(extra));
  }

  public variantId: string;

  public name: string;

  public price: number;

  public extras: Array<DishVariantExtraModel>;

  public clone(): DishVariantModel {
    return new DishVariantModel({
      variantId: this.variantId,
      name: this.name,
      price: this.price,
      extras: this.extras?.map((extra) => extra?.clone()),
    });
  }
}
