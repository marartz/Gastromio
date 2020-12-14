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

    public class RegularOpeningPeriod : OpeningPeriod
    {
        public RegularOpeningPeriod(int dayOfWeek, TimeSpan start, TimeSpan end) : base(start, end)
        {
            DayOfWeek = dayOfWeek;
        }
        
        public int DayOfWeek { get; }
    }

    public class DeviatingOpeningPeriod : OpeningPeriod
    {
        public DeviatingOpeningPeriod(DateTime date, TimeSpan start, TimeSpan end) : base(start, end)
        {
            Date = date;
        }
        
        public DateTime Date { get; }
    }
}
