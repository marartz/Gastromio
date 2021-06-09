using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class DeliveryInfo
    {
        public DeliveryInfo(bool enabled, int? averageTime, decimal? minimumOrderValue, decimal? maximumOrderValue,
            decimal? costs)
        {
            if (enabled)
            {
                ValidateAverageTime(averageTime);
                ValidateMinimumOrderValue(minimumOrderValue);
                ValidateMaximumOrderValue(maximumOrderValue);
                ValidateCosts(costs);
            }

            Enabled = enabled;
            AverageTime = averageTime;
            MinimumOrderValue = minimumOrderValue;
            MaximumOrderValue = maximumOrderValue;
            Costs = costs;
        }

        public bool Enabled { get; }

        public int? AverageTime { get; }

        public decimal? MinimumOrderValue { get; }

        public decimal? MaximumOrderValue { get; }

        public decimal? Costs { get; }

        private static void ValidateAverageTime(int? averageTime)
        {
            if (averageTime.HasValue && averageTime.Value < 5)
                throw DomainException.CreateFrom(new RestaurantAverageDeliveryTimeTooLowFailure());
            if (averageTime.HasValue && averageTime.Value > 120)
                throw DomainException.CreateFrom(new RestaurantAverageDeliveryTimeTooHighFailure());
        }

        private static void ValidateMinimumOrderValue(decimal? minimumOrderValue)
        {
            if (minimumOrderValue.HasValue && minimumOrderValue < 0)
                throw DomainException.CreateFrom(new RestaurantMinimumDeliveryOrderValueTooLowFailure());
            if (minimumOrderValue.HasValue && minimumOrderValue > 50)
                throw DomainException.CreateFrom(new RestaurantMinimumDeliveryOrderValueTooHighFailure());
        }

        private static void ValidateMaximumOrderValue(decimal? maximumOrderValue)
        {
            if (maximumOrderValue.HasValue && maximumOrderValue < 0)
                throw DomainException.CreateFrom(new RestaurantMaximumDeliveryOrderValueTooLowFailure());
        }

        private static void ValidateCosts(decimal? costs)
        {
            if (costs.HasValue && costs < 0)
                throw DomainException.CreateFrom(new RestaurantDeliveryCostsTooLowFailure());
            if (costs.HasValue && costs > 10)
                throw DomainException.CreateFrom(new RestaurantDeliveryCostsTooHighFailure());
        }
    }
}
