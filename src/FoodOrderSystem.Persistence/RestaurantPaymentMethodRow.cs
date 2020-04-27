using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderSystem.Persistence
{
    [Table("RestaurantPaymentMethod")]
    public class RestaurantPaymentMethodRow
    {
        public Guid RestaurantId { get; set; }
        public virtual RestaurantRow Restaurant { get; set; }
        public Guid PaymentMethodId { get; set; }
        public virtual PaymentMethodRow PaymentMethod { get; set; }
    }
}
