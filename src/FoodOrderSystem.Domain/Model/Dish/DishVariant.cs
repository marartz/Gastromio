using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FoodOrderSystem.Domain.Model.Dish
{
    public class DishVariant
    {
        public DishVariant(Guid variantId, string name, decimal price, IList<DishVariantExtra> extras)
        {
            VariantId = variantId;
            Name = name;
            Price = price;
        }

        public Guid VariantId { get; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
    }
}
