namespace FoodOrderSystem.Domain.Model.PaymentMethod
{
    public class PaymentMethod
    {
        public PaymentMethod(PaymentMethodId id)
        {
            Id = id;
        }
        
        public PaymentMethod(PaymentMethodId id, string name, string description)
            : this(id)
        {
            Name = name;
            Description = description;
        }

        public PaymentMethodId Id { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Result<bool> Change(string name, string description)
        {
            Name = name;
            Description = description;
            return SuccessResult<bool>.Create(true);
        }
    }
}
