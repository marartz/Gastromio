using System;
using System.Collections.Generic;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.DishCategory;
using FoodOrderSystem.Core.Domain.Model.Restaurant;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Domain.Model.Dish
{
    public class DishFactory : IDishFactory
    {
        public Result<Dish> Create(
            RestaurantId restaurantId,
            DishCategoryId categoryId,
            string name,
            string description,
            string productInfo,
            int orderNo,
            IEnumerable<DishVariant> variants,
            UserId createdBy
        )
        {
            if (restaurantId.Value == Guid.Empty)
                return FailureResult<Dish>.Create(FailureResultCode.RequiredFieldEmpty, nameof(restaurantId));
            if (categoryId.Value == Guid.Empty)
                return FailureResult<Dish>.Create(FailureResultCode.RequiredFieldEmpty, nameof(categoryId));

            var dish = new Dish(
                new DishId(Guid.NewGuid()),
                restaurantId,
                categoryId,
                DateTime.UtcNow,
                createdBy,
                DateTime.UtcNow,
                createdBy
            );

            var tempResult = dish.ChangeName(name, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<Dish>();

            tempResult = dish.ChangeDescription(description, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<Dish>();

            tempResult = dish.ChangeProductInfo(productInfo, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<Dish>();

            tempResult = dish.ChangeOrderNo(orderNo, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<Dish>();

            foreach (var variant in variants)
            {
                tempResult = dish.AddVariant(variant.VariantId, variant.Name, variant.Price, createdBy);
                if (tempResult.IsFailure)
                    return tempResult.Cast<Dish>();
            }

            return SuccessResult<Dish>.Create(dish);
        }
    }
}