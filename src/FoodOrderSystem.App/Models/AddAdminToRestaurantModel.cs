using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class AddAdminToRestaurantModel
    {
        [Required]
        public Guid UserId { get; set; }
    }
}
