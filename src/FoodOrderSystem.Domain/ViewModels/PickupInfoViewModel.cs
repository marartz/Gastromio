using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class PickupInfoViewModel
    {
        public int AverageTime { get; set; }
        
        public decimal? MinimumOrderValue { get; set; }
        
        public string MinimumOrderValueText { get; set; }
        
        public decimal? MaximumOrderValue { get; set; }
        
        public string MaximumOrderValueText { get; set; }
        
        public string HygienicHandling { get; set; }
        
        public static PickupInfoViewModel FromPickupInfo(PickupInfo pickupInfo)
        {
            return new PickupInfoViewModel
            {
                AverageTime = (int)pickupInfo.AverageTime.TotalMinutes, 
                MinimumOrderValue = pickupInfo.MinimumOrderValue,
                MinimumOrderValueText = pickupInfo.MinimumOrderValue?.ToString("0.00"),
                MaximumOrderValue = pickupInfo.MaximumOrderValue,
                MaximumOrderValueText = pickupInfo.MaximumOrderValue?.ToString("0.00"),
                HygienicHandling = pickupInfo.HygienicHandling
            };
        }
    }
}