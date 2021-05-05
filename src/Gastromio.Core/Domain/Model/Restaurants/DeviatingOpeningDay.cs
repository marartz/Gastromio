using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class DeviatingOpeningDay : OpeningDay<DeviatingOpeningDay>
    {
        public DeviatingOpeningDay(Date date, DeviatingOpeningDayStatus status, OpeningPeriods openingPeriods)
            : base(openingPeriods)
        {
            Date = date;
            Status = status;
        }

        public Date Date { get; }

        public DeviatingOpeningDayStatus Status { get; }

        public DeviatingOpeningDay ChangeStatus(DeviatingOpeningDayStatus status)
        {
            if (OpeningPeriods?.Count > 0)
                throw DomainException.CreateFrom(new RestaurantDeviatingOpeningDayHasStillOpenPeriodsFailure());
            return new DeviatingOpeningDay(Date, status, OpeningPeriods);
        }

        protected override DeviatingOpeningDay CreateSpecificOpeningDayWith(OpeningPeriods openingPeriods)
        {
            return new DeviatingOpeningDay(Date, Status, openingPeriods);
        }
    }
}
