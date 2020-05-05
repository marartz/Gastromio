using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FoodOrderSystem.Domain.Model.Dish
{
    public class DishVariant
    {
        private IList<DishVariantExtra> extras;

        public DishVariant(Guid variantId, string name, decimal price, IList<DishVariantExtra> extras)
        {
            VariantId = variantId;
            Name = name;
            Price = price;
            this.extras = extras;
        }

        public Guid VariantId { get; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public IReadOnlyList<DishVariantExtra> Extras => new ReadOnlyCollection<DishVariantExtra>(extras);

        public void ChangeName(string name)
        {
            Name = name;
        }

        public void ChangePrice(decimal price)
        {
            Price = price;
        }

        public void AddExtra(Guid extraId, string name, string productInfo, decimal price)
        {
            if (extras.Any(en => en.ExtraId == extraId))
                throw new InvalidOperationException("variant extra already exists");
            extras.Add(new DishVariantExtra(extraId, name, productInfo, price));
        }

        public void RemoveExtra(Guid extraId)
        {
            var extra = extras.FirstOrDefault(en => en.ExtraId == extraId);
            if (extra == null)
                throw new InvalidOperationException("extra does not exist");
            extras.Remove(extra);
        }

        public void ReplaceExtras(IList<DishVariantExtra> extras)
        {
            this.extras = extras;
        }
    }
}
