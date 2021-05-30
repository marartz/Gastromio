using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class DishVariant
    {
        private const int MaxNameLength = 40;
        private const decimal MaxVariantPrice = 200;

        public DishVariant(DishVariantId id, string name, decimal price)
        {
            if (name != null && name.Length > MaxNameLength)
                throw DomainException.CreateFrom(new DishVariantNameTooLongFailure(MaxNameLength));
            if (!(price > 0))
                throw DomainException.CreateFrom(new DishVariantPriceIsNegativeOrZeroFailure());
            if (price > MaxVariantPrice)
                throw DomainException.CreateFrom(new DishVariantPriceIsTooBigFailure(MaxVariantPrice));

            Id = id;
            Name = name;
            Price = price;
        }

        public DishVariantId Id { get; }

        public string Name { get; }

        public decimal Price { get; }
    }
}
