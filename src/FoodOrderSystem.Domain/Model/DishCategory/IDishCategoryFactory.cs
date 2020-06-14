using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public interface IDishCategoryFactory
    {
        Result<DishCategory> Create(
            RestaurantId restaurantId,
            string name,
            int orderNo,
            UserId createdBy
        );
    }
}