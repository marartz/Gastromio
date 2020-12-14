using System;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public abstract class OpeningPeriod
    {
        public const double EarliestOpeningTime = 4d;

        protected OpeningPeriod(TimeSpan start, TimeSpan end)
        {
            if (end.TotalHours >= EarliestOpeningTime)
            {
                Start = start;
                End = end;
            }
            else
            {
                Start = start;
                End = TimeSpan.FromHours(end.TotalHours + 24d);
            }
        }

        public TimeSpan Start { get; }
        
        public TimeSpan End { get; }
    }
}
