using System;

namespace FoodOrderSystem.Persistence
{
    public class RestaurantPaymentMethodRow
    {
        public Guid RestaurantId { get; set; }
        public virtual RestaurantRow Restaurant { get; set; }
        public Guid PaymentMethodId { get; set; }
        public virtual PaymentMethodRow PaymentMethod { get; set; }
    }
}
