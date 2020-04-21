using System;

namespace FoodOrderSystem.Persistence
{
    public class DeliveryTimeRow
    {
        public Guid RestaurantId { get; set; }

        public virtual RestaurantRow Restaurant { get; set; }

        public int DayOfWeek { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }
    }
}
