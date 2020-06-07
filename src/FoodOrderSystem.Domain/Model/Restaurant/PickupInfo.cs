using System;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class PickupInfo
    {
        public PickupInfo(TimeSpan averageTime, decimal? minimumOrderValue, decimal? maximumOrderValue,
            string hygienicHandling)
        {
            AverageTime = averageTime;
            MinimumOrderValue = minimumOrderValue;
            MaximumOrderValue = maximumOrderValue;
            HygienicHandling = hygienicHandling;
        }

        public TimeSpan AverageTime { get; }
        public decimal? MinimumOrderValue { get; }
        public decimal? MaximumOrderValue { get; }
        public string HygienicHandling { get; }
    }
}