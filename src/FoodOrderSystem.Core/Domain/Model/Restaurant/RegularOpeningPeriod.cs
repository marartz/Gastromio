using System;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public class RegularOpeningPeriod : OpeningPeriod
    {
        public RegularOpeningPeriod(int dayOfWeek, TimeSpan start, TimeSpan end) : base(start, end)
        {
            DayOfWeek = dayOfWeek;
        }
        
        public int DayOfWeek { get; }
    }
}