using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Common;

namespace Gastromio.Domain.TestKit.Domain.Model.Restaurants
{
    public class DeliveryInfoBuilder : TestObjectBuilderBase<DeliveryInfo>
    {
        protected override void AddDefaultConstraints()
        {
            WithRangeConstrainedDecimalConstructorArgumentFor("averageTime", 5, 120);
            WithRangeConstrainedDecimalConstructorArgumentFor("minimumOrderValue", 0, 50);
            WithRangeConstrainedDecimalConstructorArgumentFor("maximumOrderValue", 0, 1000);
            WithRangeConstrainedDecimalConstructorArgumentFor("maximumOrderValue", 0, 10);
        }

        public DeliveryInfoBuilder WithEnabled(bool enabled)
        {
            WithConstantConstructorArgumentFor("enabled", enabled);
            return this;
        }

        public DeliveryInfoBuilder WithAverageTime(int? averageTime)
        {
            WithConstantConstructorArgumentFor("averageTime", averageTime);
            return this;
        }

        public DeliveryInfoBuilder WithMinimumOrderValue(decimal? minimumOrderValue)
        {
            WithConstantConstructorArgumentFor("minimumOrderValue", minimumOrderValue);
            return this;
        }

        public DeliveryInfoBuilder WithMaximumOrderValue(decimal? maximumOrderValue)
        {
            WithConstantConstructorArgumentFor("maximumOrderValue", maximumOrderValue);
            return this;
        }

        public DeliveryInfoBuilder WithCosts(decimal? costs)
        {
            WithConstantConstructorArgumentFor("costs", costs);
            return this;
        }
    }
}
