using System;

namespace FoodOrderSystem.Domain.Model.PaymentMethod
{
    public class PaymentMethodId : ValueType<Guid>
    {
        public PaymentMethodId(Guid value) : base(value)
        {
        }
    }
}
