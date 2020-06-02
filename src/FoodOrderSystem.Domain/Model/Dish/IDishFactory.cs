using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.Dish
{
    public interface IDishFactory
    {
        Result<Dish> Create(RestaurantId restaurantId, DishCategoryId categoryId, string name, string description,
            string productInfo, IEnumerable<DishVariant> variants);
    }
}