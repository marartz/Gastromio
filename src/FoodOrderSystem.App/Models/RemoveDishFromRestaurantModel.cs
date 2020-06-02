using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class RemoveDishFromRestaurantModel
    {
        public Guid DishCategoryId { get; set; }
        public Guid DishId { get; set; }
    }
}
