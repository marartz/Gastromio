using System.Collections.Generic;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public class DeviatingOpeningDay : OpeningDay
    {
        public DeviatingOpeningDay(Date date, DeviatingOpeningDayStatus status, IEnumerable<OpeningPeriod> openingPeriods) : base(openingPeriods)
        {
            Date = date;
            Status = status;
        }

        public Date Date { get; }

        public DeviatingOpeningDayStatus Status { get; }
    }
}