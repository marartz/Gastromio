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
        public string Name { get; private set; }
        public string Description { get; private set; }

        public void Change(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
