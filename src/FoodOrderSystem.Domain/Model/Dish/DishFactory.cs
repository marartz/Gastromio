using System;
using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.Dish
{
    public class DishFactory : IDishFactory
    {
        public Result<Dish> Create(RestaurantId restaurantId, DishCategoryId categoryId, string name,
            string description, string productInfo, int orderNo, IEnumerable<DishVariant> variants)
        {
            if (restaurantId.Value == Guid.Empty)
                return FailureResult<Dish>.Create(FailureResultCode.RequiredFieldEmpty, nameof(restaurantId));
            if (categoryId.Value == Guid.Empty)
                return FailureResult<Dish>.Create(FailureResultCode.RequiredFieldEmpty, nameof(categoryId));

            var dish = new Dish(new DishId(Guid.NewGuid()), restaurantId, categoryId);

            var tempResult = dish.ChangeName(name);
            if (tempResult.IsFailure)
                return tempResult.Cast<Dish>();

            tempResult = dish.ChangeDescription(description);
            if (tempResult.IsFailure)
                return tempResult.Cast<Dish>();

            tempResult = dish.ChangeProductInfo(productInfo);
            if (tempResult.IsFailure)
                return tempResult.Cast<Dish>();

            tempResult = dish.ChangeOrderNo(orderNo);
            if (tempResult.IsFailure)
                return tempResult.Cast<Dish>();

            foreach (var variant in variants)
            {
                tempResult = dish.AddVariant(variant.VariantId, variant.Name, variant.Price);
                if (tempResult.IsFailure)
                    return tempResult.Cast<Dish>();
            }

            return SuccessResult<Dish>.Create(dish);
        }
    }
}