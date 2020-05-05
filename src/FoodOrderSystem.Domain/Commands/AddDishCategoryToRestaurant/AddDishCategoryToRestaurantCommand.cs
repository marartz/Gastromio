using FoodOrderSystem.Domain.Model.Restaurant;
using System;

namespace FoodOrderSystem.Domain.Commands.AddDishCategoryToRestaurant
{
    public class AddDishCategoryToRestaurantCommand : ICommand<Guid>
    {
        public AddDishCategoryToRestaurantCommand(RestaurantId restaurantId, string name)
        {
            RestaurantId = restaurantId;
            Name = name;
        }

        public RestaurantId RestaurantId { get; }
        public string Name { get; }
    }
}
