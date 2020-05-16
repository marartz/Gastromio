import { DishModel } from '../dish-category/dish.model';
import { DishVariantModel } from '../dish-category/dish-variant.model';
import { Guid } from 'guid-typescript';

export class OrderedDishModel {
  constructor() {
    this.itemId = Guid.create().toString();
  }

  public itemId: string;

  public dish: DishModel;

  public variant: DishVariantModel;

  public count: number;

  public remarks: string;

  public getPrice(): number {
    return this.count * this.variant.price;
  }

  public getPriceText(): string {
    let val = this.getPrice();
    return val.toLocaleString('de', { minimumFractionDigits: 2 });
  }

  public hasMultipleVariants(): boolean {
    return this.dish.variants !== undefined && this.dish.variants.length > 1;
  }

}
