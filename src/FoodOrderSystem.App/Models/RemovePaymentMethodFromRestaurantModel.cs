using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class RemovePaymentMethodFromRestaurantModel
    {
        [Required]
        public Guid PaymentMethodId { get; set; }
    }
}
