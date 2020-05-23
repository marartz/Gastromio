using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public class DishCategory
    {
        public DishCategory(DishCategoryId id, RestaurantId restaurantId)
        {
            Id = id;
            RestaurantId = restaurantId;
        }
        
        public DishCategory(DishCategoryId id, RestaurantId restaurantId, string name)
            : this(id, restaurantId)
        {
            Name = name;
        }

        public DishCategoryId Id { get; }
        public RestaurantId RestaurantId { get; }
        public string Name { get; private set; }

        public Result<bool> ChangeName(string name)
        {
            Name = name;
            return SuccessResult<bool>.Create(true);
        }
    }
}
