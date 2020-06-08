using FoodOrderSystem.Domain.ViewModels;
using System;

namespace FoodOrderSystem.App.Models
{
    public class AddOrChangeDishOfRestaurantModel
    {
        public Guid DishCategoryId { get; set; }
        public DishViewModel Dish { get; set; }
    }
}
