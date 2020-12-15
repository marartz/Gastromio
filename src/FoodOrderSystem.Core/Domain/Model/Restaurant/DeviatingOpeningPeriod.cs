using System;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public class DeviatingOpeningPeriod : OpeningPeriod
    {
        public DeviatingOpeningPeriod(Date date, TimeSpan start, TimeSpan end) : base(start, end)
        {
            Date = date;
        }
        
        public Date Date { get; }
    }
}