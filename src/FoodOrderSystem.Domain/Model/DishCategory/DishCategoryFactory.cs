using System;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public class DishCategoryFactory : IDishCategoryFactory
    {
        public Result<DishCategory> Create(
            RestaurantId restaurantId,
            string name,
            int orderNo,
            UserId createdBy
        )
        {
            var dishCategory = new DishCategory(
                new DishCategoryId(Guid.NewGuid()),
                restaurantId,
                DateTime.UtcNow,
                createdBy,                    
                DateTime.UtcNow,
                createdBy                    
            );
            
            var tempResult = dishCategory.ChangeName(name, createdBy);
            if (tempResult.IsFailure)
                return tempResult.Cast<DishCategory>();

            tempResult = dishCategory.ChangeOrderNo(orderNo, createdBy);
            return tempResult.IsFailure ? tempResult.Cast<DishCategory>() : SuccessResult<DishCategory>.Create(dishCategory);
        }
    }
}