using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class DeliveryTimeViewModel
    {
        public DeliveryTimeViewModel(
            int dayOfWeek,
            int start,
            int end
        )
        {
            DayOfWeek = dayOfWeek;
            Start = start;
            End = end;
        }

        public int DayOfWeek { get; }

        public int Start { get; }

        public int End { get; }

        public static DeliveryTimeViewModel FromDeliveryTime(DeliveryTime deliveryTime)
        {
            return new DeliveryTimeViewModel(deliveryTime.DayOfWeek, (int)deliveryTime.Start.TotalMinutes, (int)deliveryTime.End.TotalMinutes);
        }
    }
}
