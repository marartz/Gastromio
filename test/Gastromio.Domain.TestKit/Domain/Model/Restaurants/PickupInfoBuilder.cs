using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Common;

namespace Gastromio.Domain.TestKit.Domain.Model.Restaurants
{
    public class PickupInfoBuilder : TestObjectBuilderBase<PickupInfo>
    {
        protected override void AddDefaultConstraints()
        {
            WithRangeConstrainedDecimalConstructorArgumentFor("averageTime", 5, 120);
            WithRangeConstrainedDecimalConstructorArgumentFor("minimumOrderValue", 0, 50);
            WithRangeConstrainedDecimalConstructorArgumentFor("maximumOrderValue", 0, 1000);
        }

        public PickupInfoBuilder WithEnabled(bool enabled)
        {
            WithConstantConstructorArgumentFor("enabled", enabled);
            return this;
        }

        public PickupInfoBuilder WithAverageTime(int? averageTime)
        {
            WithConstantConstructorArgumentFor("averageTime", averageTime);
            return this;
        }

        public PickupInfoBuilder WithMinimumOrderValue(decimal? minimumOrderValue)
        {
            WithConstantConstructorArgumentFor("minimumOrderValue", minimumOrderValue);
            return this;
        }

        public PickupInfoBuilder WithMaximumOrderValue(decimal? maximumOrderValue)
        {
            WithConstantConstructorArgumentFor("maximumOrderValue", maximumOrderValue);
            return this;
        }

    }
}
