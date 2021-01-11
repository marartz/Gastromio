export class OrderedDishInfoModel {

  constructor(init?: Partial<OrderedDishInfoModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public itemId: string;

  public dishId: string;

  public dishName: string;

  public variantId: string;

  public variantName: string;

  public variantPrice: number;

  public count: number;

  public remarks: string;

  public clone(): OrderedDishInfoModel {
    return new OrderedDishInfoModel({
      itemId: this.itemId,
      dishId: this.dishId,
      dishName: this.dishName,
      variantId: this.variantId,
      variantName: this.variantName,
      variantPrice: this.variantPrice,
      count: this.count,
      remarks: this.remarks
    });
  }

}
