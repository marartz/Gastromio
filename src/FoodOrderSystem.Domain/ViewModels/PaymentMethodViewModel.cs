using FoodOrderSystem.Domain.Model.PaymentMethod;
using System;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class PaymentMethodViewModel
    {
        public PaymentMethodViewModel(
            Guid id,
            string name,
            string description
        )
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }

        public static PaymentMethodViewModel FromPaymentMethod(PaymentMethod paymentMethod)
        {
            return new PaymentMethodViewModel(paymentMethod.Id.Value, paymentMethod.Name, paymentMethod.Description);
        }
    }
}
