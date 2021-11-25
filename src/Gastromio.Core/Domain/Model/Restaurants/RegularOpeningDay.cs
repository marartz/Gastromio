using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class RegularOpeningDay : OpeningDay<RegularOpeningDay>
    {
        public RegularOpeningDay(int dayOfWeek, OpeningPeriods openingPeriods)
            : base(openingPeriods)
        {
            if (dayOfWeek < 0 || dayOfWeek > 6)
                throw DomainException.CreateFrom(new DayOfWeekIsInvalidFailure());
            DayOfWeek = dayOfWeek;
        }

        public int DayOfWeek { get; }

        protected override RegularOpeningDay CreateSpecificOpeningDayWith(OpeningPeriods openingPeriods)
        {
            return new RegularOpeningDay(DayOfWeek, openingPeriods);
        }
    }
}
