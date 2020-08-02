using System;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Domain.Model.Dish;
using FoodOrderSystem.Core.Domain.Model.DishCategory;
using FoodOrderSystem.Core.Domain.Model.Restaurant;


namespace FoodOrderSystem.Core.Application.Commands.AddOrChangeDishOfRestaurant
{
    public class AddOrChangeDishOfRestaurantCommand : ICommand<Guid>
    {
        public AddOrChangeDishOfRestaurantCommand(RestaurantId restaurantId, DishCategoryId dishCategoryId,
            DishDTO dish)
        {
            RestaurantId = restaurantId;
            DishCategoryId = dishCategoryId;
            Dish = dish;
        }

        public RestaurantId RestaurantId { get; }
        public DishCategoryId DishCategoryId { get; }
        public DishDTO Dish { get; }
    }
}
