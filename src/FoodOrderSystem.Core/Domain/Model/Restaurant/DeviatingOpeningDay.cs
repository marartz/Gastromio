using System.Collections.Generic;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public class DeviatingOpeningDay : OpeningDay
    {
        public DeviatingOpeningDay(Date date, IEnumerable<OpeningPeriod> openingPeriods) : base(openingPeriods)
        {
            Date = date;
        }

        public Date Date { get; }
    }
}