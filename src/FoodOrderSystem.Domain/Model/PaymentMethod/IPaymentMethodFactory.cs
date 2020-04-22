namespace FoodOrderSystem.Domain.Model.PaymentMethod
{
    public interface IPaymentMethodFactory
    {
        PaymentMethod Create(string name, string description);
    }
}
