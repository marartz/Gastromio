using System;
using System.Collections.Generic;

namespace FoodOrderSystem.Core.Domain.Model.Dish
{
    public class DishVariant
    {
        public DishVariant(Guid variantId, string name, decimal price)
        {
            VariantId = variantId;
            Name = name;
            Price = price;
        }

        public Guid VariantId { get; }
        
        public string Name { get; }
        
        public decimal Price { get; }
    }
}
