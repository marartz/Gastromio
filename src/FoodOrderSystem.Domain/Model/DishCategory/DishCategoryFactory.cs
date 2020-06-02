using System;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public class DishCategoryFactory : IDishCategoryFactory
    {
        public Result<DishCategory> Create(RestaurantId restaurantId, string name)
        {
            var dishCategory = new DishCategory(new DishCategoryId(Guid.NewGuid()), restaurantId);
            var tempResult = dishCategory.ChangeName(name);
            return tempResult.IsFailure ? tempResult.Cast<DishCategory>() : SuccessResult<DishCategory>.Create(dishCategory);
        }
    }
}