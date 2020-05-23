using System;

namespace FoodOrderSystem.Domain.Model.PaymentMethod
{
    public class PaymentMethodFactory : IPaymentMethodFactory
    {
        public Result<PaymentMethod> Create(string name, string description)
        {
            var paymentMethod = new PaymentMethod(new PaymentMethodId(Guid.NewGuid()));

            var tempResult = paymentMethod.Change(name, description);
            if (tempResult.IsFailure)
                return tempResult.Cast<PaymentMethod>();

            return SuccessResult<PaymentMethod>.Create(paymentMethod);
        }
    }
}
