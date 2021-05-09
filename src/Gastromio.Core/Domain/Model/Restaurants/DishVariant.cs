using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class DishVariant
    {
        public DishVariant(DishVariantId id, string name, decimal price)
        {
            if (name != null && name.Length > 40)
                throw DomainException.CreateFrom(new DishVariantNameTooLongFailure(40));
            if (!(price > 0))
                throw DomainException.CreateFrom(new DishVariantPriceIsNegativeOrZeroFailure());
            if (price > 200)
                throw DomainException.CreateFrom(new DishVariantPriceIsTooBigFailure());

            Id = id;
            Name = name;
            Price = price;
        }

        public DishVariantId Id { get; }

        public string Name { get; }

        public decimal Price { get; }
    }
}
