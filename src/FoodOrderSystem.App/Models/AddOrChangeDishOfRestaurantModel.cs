using System;
using FoodOrderSystem.Core.Application.DTOs;

namespace FoodOrderSystem.App.Models
{
    public class AddOrChangeDishOfRestaurantModel
    {
        public Guid DishCategoryId { get; set; }
        public DishDTO Dish { get; set; }
    }
}
