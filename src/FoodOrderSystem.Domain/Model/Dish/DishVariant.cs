using System.Collections.Generic;

namespace FoodOrderSystem.Domain.Model.Dish
{
    public class DishVariant
    {
        public DishVariant(string name, decimal price, IList<DishVariantExtra> extras)
        {
            Name = name;
            Price = price;
            Extras = extras;
        }

        public string Name { get; }
        public decimal Price { get; }
        public IList<DishVariantExtra> Extras { get; }
    }
}
