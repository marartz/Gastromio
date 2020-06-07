namespace FoodOrderSystem.Persistence.MongoDB
{
    public class DeliveryTimeModel
    {
        public int DayOfWeek { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }
    }
}