using System;
using System.ComponentModel.DataAnnotations;

namespace FoodOrderSystem.App.Models
{
    public class RemovePaymentMethodFromRestaurantModel
    {
        public Guid PaymentMethodId { get; set; }
    }
}
