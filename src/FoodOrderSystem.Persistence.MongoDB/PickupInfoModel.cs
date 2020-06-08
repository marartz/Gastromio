﻿namespace FoodOrderSystem.Persistence.MongoDB
{
    public class PickupInfoModel
    {
        public int AverageTime { get; set; }

        public double? MinimumOrderValue { get; set; }
        
        public double? MaximumOrderValue { get; set; }
        
        public string HygienicHandling { get; set; }
    }
}