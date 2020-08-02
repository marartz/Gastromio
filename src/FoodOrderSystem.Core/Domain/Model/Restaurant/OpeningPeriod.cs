using System;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public class OpeningPeriod
    {
        public OpeningPeriod(int dayOfWeek, TimeSpan start, TimeSpan end)
        {
            DayOfWeek = dayOfWeek;
            Start = start;
            End = end;
        }

        public int DayOfWeek { get; }
        
        public TimeSpan Start { get; }
        
        public TimeSpan End { get; }
    }
}
