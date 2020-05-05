using FoodOrderSystem.Domain.Model.PaymentMethod;
using System;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class PaymentMethodViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public static PaymentMethodViewModel FromPaymentMethod(PaymentMethod paymentMethod)
        {
            return new PaymentMethodViewModel
            {
                Id = paymentMethod.Id.Value, 
                Name = paymentMethod.Name,
                Description = paymentMethod.Description
            };
        }
    }
}
