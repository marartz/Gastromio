using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Model.Dish
{
    public interface IDishFactory
    {
        Result<Dish> Create(
            RestaurantId restaurantId,
            DishCategoryId categoryId,
            string name,
            string description,
            string productInfo,
            int orderNo,
            IEnumerable<DishVariant> variants,
            UserId createdBy
        );
    }
}