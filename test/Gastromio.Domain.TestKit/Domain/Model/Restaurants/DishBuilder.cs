using System.Collections.Generic;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Common;

namespace Gastromio.Domain.TestKit.Domain.Model.Restaurants
{
    public class DishBuilder : TestObjectBuilderBase<Dish>
    {
        public DishBuilder WithValidConstrains()
        {
            WithName("dish-name");
            WithDescription("dish-description");
            WithProductInfo("dish-product-info");
            WithOrderNo(1);
            return this;
        }

        public DishBuilder WithId(DishId id)
        {
            WithConstantConstructorArgumentFor("id", id);
            return this;
        }

        public DishBuilder WithName(string name)
        {
            WithConstantConstructorArgumentFor("name", name);
            return this;
        }

        public DishBuilder WithDescription(string description)
        {
            WithConstantConstructorArgumentFor("description", description);
            return this;
        }

        public DishBuilder WithProductInfo(string productInfo)
        {
            WithConstantConstructorArgumentFor("productInfo", productInfo);
            return this;
        }

        public DishBuilder WithOrderNo(int orderNo)
        {
            WithConstantConstructorArgumentFor("orderNo", orderNo);
            return this;
        }

        public DishBuilder WithoutVariants()
        {
            return WithVariants(new List<DishVariant>());
        }

        public DishBuilder WithVariants(IEnumerable<DishVariant> variants)
        {
            return WithVariants(new DishVariants(variants));
        }

        public DishBuilder WithVariants(DishVariants variants)
        {
            WithConstantConstructorArgumentFor("variants", variants);
            return this;
        }
    }
}
