using System;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class DeliveryInfo
    {
        public DeliveryInfo(bool enabled, TimeSpan? averageTime, decimal? minimumOrderValue, decimal? maximumOrderValue,
            decimal? costs)
        {
            Enabled = enabled;
            AverageTime = averageTime;
            MinimumOrderValue = minimumOrderValue;
            MaximumOrderValue = maximumOrderValue;
            Costs = costs;
        }

        public bool Enabled { get; }
        public TimeSpan? AverageTime { get; }
        public decimal? MinimumOrderValue { get; }
        public decimal? MaximumOrderValue { get; }
        public decimal? Costs { get; }
    }
}