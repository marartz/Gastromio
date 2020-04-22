using System;

namespace FoodOrderSystem.App.Models
{
    public class PaymentMethodModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
