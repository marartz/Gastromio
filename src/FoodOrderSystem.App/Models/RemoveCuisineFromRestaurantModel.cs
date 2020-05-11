using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class RemoveCuisineFromRestaurantModel
    {
        [Required]
        public Guid CuisineId { get; set; }
    }
}
