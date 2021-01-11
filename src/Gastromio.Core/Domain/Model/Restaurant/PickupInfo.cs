using System;

namespace Gastromio.Core.Domain.Model.Restaurant
{
    public class PickupInfo
    {
        public PickupInfo(bool enabled, int? averageTime, decimal? minimumOrderValue, decimal? maximumOrderValue)
        {
            Enabled = enabled;
            AverageTime = averageTime;
            MinimumOrderValue = minimumOrderValue;
            MaximumOrderValue = maximumOrderValue;
        }

        public bool Enabled { get; }
        
        public int? AverageTime { get; }
        
        public decimal? MinimumOrderValue { get; }
        
        public decimal? MaximumOrderValue { get; }
    }
}