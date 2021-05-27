using System;
using Gastromio.Core.Domain.Model.Dishes;
using Gastromio.Domain.TestKit.Common;

namespace Gastromio.Domain.TestKit.Domain.Model.Dishes
{
    public class DishVariantBuilder : TestObjectBuilderBase<DishVariant>
    {
        public DishVariantBuilder WithValidConstrains()
        {
            WithRangeConstrainedDecimalConstructorArgumentFor("price", 0, 200);
            return this;
        }

        public DishVariantBuilder WithVariantId(Guid variantId)
        {
            WithConstantConstructorArgumentFor("variantId", variantId);
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
