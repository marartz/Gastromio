using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class RemoveDishFromRestaurantModel
    {
        [Required]
        public Guid DishCategoryId { get; set; }
        [Required]
        public Guid DishId { get; set; }
    }
}
