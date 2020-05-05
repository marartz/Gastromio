using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.ViewModels;
using System;

namespace FoodOrderSystem.Domain.Commands.AddOrChangeDishOfRestaurant
{
    public class AddOrChangeDishOfRestaurantCommand : ICommand<Guid>
    {
        public AddOrChangeDishOfRestaurantCommand(RestaurantId restaurantId, DishCategoryId dishCategoryId, DishViewModel dish)
        {
            RestaurantId = restaurantId;
            DishCategoryId = dishCategoryId;
            Dish = dish;
        }

        public RestaurantId RestaurantId { get; }
        public DishCategoryId DishCategoryId { get; }
        public DishViewModel Dish { get; }
    }
}
