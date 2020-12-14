using System;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class DeviatingOpeningPeriodModel
    {
        public DateTime Date { get; set; }

        public int StartTime { get; set; }

        public int EndTime { get; set; }
    }
}