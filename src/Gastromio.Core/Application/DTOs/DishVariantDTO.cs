using System;
using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.DTOs
{
    public class DishVariantDTO
    {
        public DishVariantDTO(Guid variantId, string name, decimal price)
        {
            VariantId = variantId;
            Name = name;
            Price = price;
        }

        internal DishVariantDTO(DishVariant dishVariant)
        {
            VariantId = dishVariant.Id.Value;
            Name = dishVariant.Name;
            Price = dishVariant.Price;
        }

        public Guid VariantId { get; }

        public string Name { get; }

        public decimal Price { get; }
    }
}
