using System;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.Core.Domain.Model.PaymentMethod
{
    public class PaymentMethodId : ValueType<Guid>
    {
        public PaymentMethodId(Guid value) : base(value)
        {
        }
    }
}
