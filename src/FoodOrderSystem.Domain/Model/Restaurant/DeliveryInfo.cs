using System;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public class DeliveryInfo
    {
        public DeliveryInfo(TimeSpan averageTime, decimal? minimumOrderValue, decimal? maximumOrderValue,
            decimal? costs, string hygienicHandling)
        {
            AverageTime = averageTime;
            MinimumOrderValue = minimumOrderValue;
            MaximumOrderValue = maximumOrderValue;
            Costs = costs;
            HygienicHandling = hygienicHandling;
        }
        
        public TimeSpan AverageTime { get; }
        public decimal? MinimumOrderValue { get; }
        public decimal? MaximumOrderValue { get; }
        public decimal? Costs { get; }
        public string HygienicHandling { get; }
    }
}