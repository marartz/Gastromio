using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Common;

namespace Gastromio.Domain.TestKit.Domain.Model.Restaurants
{
    public class DishVariantBuilder : TestObjectBuilderBase<DishVariant>
    {
        public DishVariantBuilder WithValidConstrains()
        {
            WithPrice(1.23m);
            return this;
        }

        public DishVariantBuilder WithId(DishVariantId id)
        {
            WithConstantConstructorArgumentFor("id", id);
            return this;
        }

        public DishVariantBuilder WithName(string name)
        {
            WithConstantConstructorArgumentFor("name", name);
            return this;
        }

        public DishVariantBuilder WithPrice(decimal price)
        {
            WithConstantConstructorArgumentFor("price", price);
            return this;
        }
    }
}
