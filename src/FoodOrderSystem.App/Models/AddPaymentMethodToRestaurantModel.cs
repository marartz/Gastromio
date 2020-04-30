using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class AddPaymentMethodToRestaurantModel
    {
        [Required]
        public Guid PaymentMethodId { get; set; }
    }
}
