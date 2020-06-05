using FoodOrderSystem.Domain.Model.Restaurant;
using System;
using FoodOrderSystem.Domain.Model.DishCategory;

namespace FoodOrderSystem.Domain.Commands.AddDishCategoryToRestaurant
{
    public class AddDishCategoryToRestaurantCommand : ICommand<Guid>
    {
        public AddDishCategoryToRestaurantCommand(RestaurantId restaurantId, string name, DishCategoryId afterCategoryId)
        {
            RestaurantId = restaurantId;
            Name = name;
            AfterCategoryId = afterCategoryId;
        }

        public RestaurantId RestaurantId { get; }
        public string Name { get; }
        public DishCategoryId AfterCategoryId { get; }
    }
}
