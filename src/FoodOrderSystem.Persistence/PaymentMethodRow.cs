using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderSystem.Persistence
{
    [Table("PaymentMethod")]
    public class PaymentMethodRow
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<RestaurantPaymentMethodRow> RestaurantPaymentMethods { get; set; } = new List<RestaurantPaymentMethodRow>();
    }
}
