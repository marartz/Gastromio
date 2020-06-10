using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class PickupInfoViewModel
    {
        public bool Enabled { get; set; }
        
        public int? AverageTime { get; set; }
        
        public decimal? MinimumOrderValue { get; set; }
        
        public string MinimumOrderValueText { get; set; }
        
        public decimal? MaximumOrderValue { get; set; }
        
        public string MaximumOrderValueText { get; set; }
        
        public static PickupInfoViewModel FromPickupInfo(PickupInfo pickupInfo)
        {
            return new PickupInfoViewModel
            {
                Enabled = pickupInfo.Enabled,
                AverageTime = (int?) pickupInfo.AverageTime?.TotalMinutes, 
                MinimumOrderValue = pickupInfo.MinimumOrderValue,
                MinimumOrderValueText = pickupInfo.MinimumOrderValue?.ToString("0.00"),
                MaximumOrderValue = pickupInfo.MaximumOrderValue,
                MaximumOrderValueText = pickupInfo.MaximumOrderValue?.ToString("0.00")
            };
        }
    }
}