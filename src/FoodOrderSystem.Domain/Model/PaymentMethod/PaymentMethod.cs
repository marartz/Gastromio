namespace FoodOrderSystem.Domain.Model.PaymentMethod
{
    public class PaymentMethod
    {
        public PaymentMethod(PaymentMethodId id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public PaymentMethodId Id { get; }
        public string Name { get; }
        public string Description { get; }
    }
}
