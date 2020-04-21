using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public class DishCategory
    {
        public DishCategory(DishCategoryId categoryId, RestaurantId restaurantId, string name)
        {
            CategoryId = categoryId;
            RestaurantId = restaurantId;
            Name = name;
        }

        public DishCategoryId CategoryId { get; }
        public RestaurantId RestaurantId { get; }
        public string Name { get; }
    }
}
