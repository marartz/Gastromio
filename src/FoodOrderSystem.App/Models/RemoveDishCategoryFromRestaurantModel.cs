using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class RemoveDishCategoryFromRestaurantModel
    {
        [Required]
        public Guid DishCategoryId { get; set; }
    }
}
