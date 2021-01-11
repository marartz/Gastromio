using System.Collections.Generic;
using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Model.Restaurant
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