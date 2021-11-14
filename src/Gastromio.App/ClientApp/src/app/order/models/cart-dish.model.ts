import { DishModel } from '../../shared/models/dish.model';
import { DishVariantModel } from '../../shared/models/dish-variant.model';

export class CartDishModel {
  constructor(
    private itemId: string,
    private dish: DishModel,
    private variant: DishVariantModel,
    private count: number,
    private remarks: string
  ) {}

  public getItemId(): string {
    return this.itemId;
  }

  public getDish(): DishModel {
    return this.dish;
  }

  public getVariant(): DishVariantModel {
    return this.variant;
  }

  public getCount(): number {
    return this.count;
  }

  public getRemarks(): string {
    return this.remarks;
  }

  public getPrice(): number {
    return this.count * this.variant.price;
  }

  public getPriceText(): string {
    const val = this.getPrice();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public clone(): CartDishModel {
    return new CartDishModel(
      this.itemId,
      this.dish?.clone(),
      this.variant?.clone(),
      this.count,
      this.remarks
    );
  }
}
