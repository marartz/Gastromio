using FoodOrderSystem.Domain.Model.PaymentMethod;

namespace FoodOrderSystem.Domain.Commands.ChangePaymentMethod
{
    public class ChangePaymentMethodCommand : ICommand
    {
        public ChangePaymentMethodCommand(PaymentMethodId paymentMethodId, string name, string description)
        {
            PaymentMethodId = paymentMethodId;
            Name = name;
            Description = description;
        }

        public PaymentMethodId PaymentMethodId { get; }
        public string Name { get; }
        public string Description { get; }
    }
}
