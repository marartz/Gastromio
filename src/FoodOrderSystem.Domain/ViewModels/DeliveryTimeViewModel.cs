using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class DeliveryTimeViewModel
    {
        public int DayOfWeek { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public static DeliveryTimeViewModel FromDeliveryTime(DeliveryTime deliveryTime)
        {
            return new DeliveryTimeViewModel
            {
                DayOfWeek = deliveryTime.DayOfWeek,
                Start = (int)deliveryTime.Start.TotalMinutes,
                End = (int)deliveryTime.End.TotalMinutes
            };
        }
    }
}
