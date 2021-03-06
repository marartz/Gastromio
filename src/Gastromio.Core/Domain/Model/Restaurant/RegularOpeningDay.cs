using System.Collections.Generic;

namespace Gastromio.Core.Domain.Model.Restaurant
{
    public class RegularOpeningDay : OpeningDay
    {
        public RegularOpeningDay(int dayOfWeek, IEnumerable<OpeningPeriod> openingPeriods) : base(openingPeriods)
        {
            DayOfWeek = dayOfWeek;
        }

        public int DayOfWeek { get; }
    }
}