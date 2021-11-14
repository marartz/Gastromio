import { DishVariantModel } from './dish-variant.model';

export class DishModel {
  constructor(init?: Partial<DishModel>) {
    if (init) {
      Object.assign(this, init);
    }
    this.variants = this.variants?.map(
      (variant) => new DishVariantModel(variant)
    );
  }

  public id: string;

  public name: string;

  public description: string;

  public productInfo: string;

  public variants: Array<DishVariantModel>;

  public clone(): DishModel {
    return new DishModel({
      id: this.id,
      name: this.name,
      description: this.description,
      productInfo: this.productInfo,
      variants: this.variants?.map((variant) => variant?.clone()),
    });
  }
}
