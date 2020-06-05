using System;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public class DishCategoryFactory : IDishCategoryFactory
    {
        public Result<DishCategory> Create(RestaurantId restaurantId, string name, int orderNo)
        {
            var dishCategory = new DishCategory(new DishCategoryId(Guid.NewGuid()), restaurantId);
            var tempResult = dishCategory.ChangeName(name);
            if (tempResult.IsFailure)
                return tempResult.Cast<DishCategory>();

            tempResult = dishCategory.ChangeOrderNo(orderNo);
            return tempResult.IsFailure ? tempResult.Cast<DishCategory>() : SuccessResult<DishCategory>.Create(dishCategory);
        }
    }
}