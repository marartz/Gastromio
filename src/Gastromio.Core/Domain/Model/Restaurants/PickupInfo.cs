using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class PickupInfo
    {
        public PickupInfo(bool enabled, int? averageTime, decimal? minimumOrderValue, decimal? maximumOrderValue)
        {
            if (enabled)
            {
                ValidateAverageTime(averageTime);
                ValidateMinimumOrderValue(minimumOrderValue);
                ValidateMaximumOrderValue(maximumOrderValue);
            }

            Enabled = enabled;
            AverageTime = averageTime;
            MinimumOrderValue = minimumOrderValue;
            MaximumOrderValue = maximumOrderValue;
        }

        public bool Enabled { get; }

        public int? AverageTime { get; }

        public decimal? MinimumOrderValue { get; }

        public decimal? MaximumOrderValue { get; }

        private static void ValidateAverageTime(int? averageTime)
        {
            if (averageTime.HasValue && averageTime.Value < 5)
                throw DomainException.CreateFrom(new RestaurantAveragePickupTimeTooLowFailure());
            if (averageTime.HasValue && averageTime.Value > 120)
                throw DomainException.CreateFrom(new RestaurantAveragePickupTimeTooHighFailure());
        }

        private static void ValidateMinimumOrderValue(decimal? minimumOrderValue)
        {
            if (minimumOrderValue.HasValue && minimumOrderValue < 0)
                throw DomainException.CreateFrom(new RestaurantMinimumPickupOrderValueTooLowFailure());
            if (minimumOrderValue.HasValue && minimumOrderValue > 50)
                throw DomainException.CreateFrom(new RestaurantMinimumPickupOrderValueTooHighFailure());
        }

        private static void ValidateMaximumOrderValue(decimal? maximumOrderValue)
        {
            if (maximumOrderValue.HasValue && maximumOrderValue < 0)
                throw DomainException.CreateFrom(new RestaurantMaximumPickupOrderValueTooLowFailure());
        }
    }
}
