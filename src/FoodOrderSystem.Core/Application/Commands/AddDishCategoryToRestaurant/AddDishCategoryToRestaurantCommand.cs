using System;
using FoodOrderSystem.Core.Domain.Model.DishCategory;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.AddDishCategoryToRestaurant
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
