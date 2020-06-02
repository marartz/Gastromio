using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public interface IDishCategoryFactory
    {
        Result<DishCategory> Create(RestaurantId restaurantId, string name);
    }
}