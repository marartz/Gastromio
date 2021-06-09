namespace Gastromio.Core.Domain.Model.PaymentMethods
{
    public class PaymentMethod
    {
        public PaymentMethod(PaymentMethodId id, string name, string description, string imageName)
        {
            Id = id;
            Name = name;
            Description = description;
            ImageName = imageName;
        }

        public PaymentMethodId Id { get; }

        public string Name { get; }

        public string Description { get; }

        public string ImageName { get; }
    }
}
