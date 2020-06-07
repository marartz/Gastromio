using System;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class PaymentMethodModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}