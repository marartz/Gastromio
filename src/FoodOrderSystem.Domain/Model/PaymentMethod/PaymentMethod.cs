using System.Security.Cryptography;

namespace FoodOrderSystem.Domain.Model.PaymentMethod
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
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageName { get; }
    }
}
