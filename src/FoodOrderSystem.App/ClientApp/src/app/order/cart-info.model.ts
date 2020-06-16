import {OrderedDishInfoModel} from './ordered-dish-info.model';

export class CartInfoModel {
    public orderType: string;
    public restaurantId: string;
    public restaurantInfo: string;
    public orderedDishes: OrderedDishInfoModel[];
}
