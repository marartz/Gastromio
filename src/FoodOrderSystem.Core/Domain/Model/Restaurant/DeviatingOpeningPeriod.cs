using System;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public class DeviatingOpeningPeriod : OpeningPeriod
    {
        public DeviatingOpeningPeriod(DateTime date, TimeSpan start, TimeSpan end) : base(start, end)
        {
            Date = date;
        }
        
        public DateTime Date { get; }
    }
}