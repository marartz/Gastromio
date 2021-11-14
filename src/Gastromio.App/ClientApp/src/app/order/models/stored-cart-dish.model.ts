export class StoredCartDishModel {
  constructor(init?: Partial<StoredCartDishModel>) {
    if (init) {
      Object.assign(this, init);
    }
  }

  public itemId: string;

  public dishId: string;

  public variantId: string;

  public count: number;

  public remarks: string;

  public clone(): StoredCartDishModel {
    return new StoredCartDishModel({
      itemId: this.itemId,
      dishId: this.dishId,
      variantId: this.variantId,
      count: this.count,
      remarks: this.remarks,
    });
  }
}
