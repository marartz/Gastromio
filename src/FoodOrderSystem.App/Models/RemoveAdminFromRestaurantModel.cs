using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class RemoveAdminFromRestaurantModel
    {
        public Guid UserId { get; set; }
    }
}
