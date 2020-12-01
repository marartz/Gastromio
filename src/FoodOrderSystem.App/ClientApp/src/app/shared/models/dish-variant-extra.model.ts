export class DishVariantExtraModel {
  constructor(init?: Partial<DishVariantExtraModel>) {
    Object.assign(this, init);
  }

  public extraId: string;

  public name: string;

  public productInfo: string;

  public price: number;
}
