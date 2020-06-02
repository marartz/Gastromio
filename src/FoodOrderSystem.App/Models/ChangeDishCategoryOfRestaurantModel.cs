using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class ChangeDishCategoryOfRestaurantModel
    {
        public Guid DishCategoryId { get; set; }
        public string Name { get; set; }
    }
}
