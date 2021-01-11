export class DishVariantExtraModel {

  constructor(init?: Partial<DishVariantExtraModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public extraId: string;

  public name: string;

  public productInfo: string;

  public price: number;

  public clone(): DishVariantExtraModel {
    return new DishVariantExtraModel({
      extraId: this.extraId,
      name: this.name,
      productInfo: this.productInfo,
      price: this.price
    });
  }

}
