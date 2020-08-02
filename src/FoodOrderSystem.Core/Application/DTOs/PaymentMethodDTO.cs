using System;
using FoodOrderSystem.Core.Domain.Model.PaymentMethod;

namespace FoodOrderSystem.Core.Application.DTOs
{
    public class PaymentMethodDTO
    {
        public PaymentMethodDTO(Guid id, string name, string description, string imageName)
        {
            Id = id;
            Name = name;
            Description = description;
            ImageName = imageName;
        }

        internal PaymentMethodDTO(PaymentMethod paymentMethod)
        {
            Id = paymentMethod.Id.Value;
            Name = paymentMethod.Name;
            Description = paymentMethod.Description;
            ImageName = paymentMethod.ImageName;
        }

        public Guid Id { get; }

        public string Name { get; }

        public string Description { get; }
        
        public string ImageName { get; }
    }
}
