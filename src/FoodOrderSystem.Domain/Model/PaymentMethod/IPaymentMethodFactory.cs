namespace FoodOrderSystem.Domain.Model.PaymentMethod
{
    public interface IPaymentMethodFactory
    {
        Result<PaymentMethod> Create(string name, string description);
    }
}
