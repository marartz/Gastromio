using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class DeliveryInfoViewModel
    {
        public int AverageTime { get; set; }

        public decimal? MinimumOrderValue { get; set; }
        
        public string MinimumOrderValueText { get; set; }

        public decimal? MaximumOrderValue { get; set; }
        
        public string MaximumOrderValueText { get; set; }

        public decimal? Costs { get; set; }
        
        public string CostsText { get; set; }

        public string HygienicHandling { get; set; }

        public static DeliveryInfoViewModel FromDeliveryInfo(DeliveryInfo deliveryInfo)
        {
            return new DeliveryInfoViewModel
            {
                AverageTime = (int) deliveryInfo.AverageTime.TotalMinutes,
                MinimumOrderValue = deliveryInfo.MinimumOrderValue,
                MinimumOrderValueText = deliveryInfo.MinimumOrderValue?.ToString("0.00"),
                MaximumOrderValue = deliveryInfo.MaximumOrderValue,
                MaximumOrderValueText = deliveryInfo.MaximumOrderValue?.ToString("0.00"),
                Costs = deliveryInfo.Costs,
                CostsText = deliveryInfo.Costs?.ToString("0.00"),
                HygienicHandling = deliveryInfo.HygienicHandling
            };
        }
    }
}