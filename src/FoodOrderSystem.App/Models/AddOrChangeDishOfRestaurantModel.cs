using FoodOrderSystem.Domain.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class AddOrChangeDishOfRestaurantModel
    {
        [Required]
        public Guid DishCategoryId { get; set; }
        [Required]
        public DishViewModel Dish { get; set; }
    }
}
