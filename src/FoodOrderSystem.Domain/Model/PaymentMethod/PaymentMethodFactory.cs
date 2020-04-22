using System;

namespace FoodOrderSystem.Domain.Model.PaymentMethod
{
    public class PaymentMethodFactory : IPaymentMethodFactory
    {
        public PaymentMethod Create(string name, string description)
        {
            return new PaymentMethod(new PaymentMethodId(Guid.NewGuid()), name, description);
        }
    }
}
