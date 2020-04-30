using FoodOrderSystem.Domain.Model.PaymentMethod;

namespace FoodOrderSystem.Domain.Commands.RemovePaymentMethod
{
    public class RemovePaymentMethodCommand : ICommand<bool>
    {
        public RemovePaymentMethodCommand(PaymentMethodId paymentMethodId)
        {
            PaymentMethodId = paymentMethodId;
        }

        public PaymentMethodId PaymentMethodId { get; }
    }
}
