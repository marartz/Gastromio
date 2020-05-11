using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class AddCuisineToRestaurantModel
    {
        [Required]
        public Guid CuisineId { get; set; }
    }
}
