using System;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public class PickupInfo
    {
        public PickupInfo(bool enabled, TimeSpan? averageTime, decimal? minimumOrderValue, decimal? maximumOrderValue)
        {
            Enabled = enabled;
            AverageTime = averageTime;
            MinimumOrderValue = minimumOrderValue;
            MaximumOrderValue = maximumOrderValue;
        }

        public bool Enabled { get; }
        
        public TimeSpan? AverageTime { get; }
        
        public decimal? MinimumOrderValue { get; }
        
        public decimal? MaximumOrderValue { get; }
    }
}