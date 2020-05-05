using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class ChangeDishCategoryOfRestaurantModel
    {
        [Required]
        public Guid DishCategoryId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
