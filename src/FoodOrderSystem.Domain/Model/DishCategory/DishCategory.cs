using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.DishCategory
{
    public class DishCategory
    {
        public DishCategory(DishCategoryId id, RestaurantId restaurantId, string name)
        {
            Id = id;
            RestaurantId = restaurantId;
            Name = name;
        }

        public DishCategoryId Id { get; }
        public RestaurantId RestaurantId { get; }
        public string Name { get; private set; }

        public void ChangeName(string name)
        {
            Name = name;
        }
    }
}
