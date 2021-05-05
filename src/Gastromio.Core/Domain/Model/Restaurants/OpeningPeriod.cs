using System;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class OpeningPeriod
    {
        public const double EarliestOpeningTime = 4d;

        public OpeningPeriod(TimeSpan start, TimeSpan end)
        {
            if (start.TotalHours < EarliestOpeningTime)
                throw DomainException.CreateFrom(new RestaurantOpeningPeriodBeginsTooEarlyFailure());
            if (end.TotalHours < EarliestOpeningTime)
                end = TimeSpan.FromHours(end.TotalHours + 24d);
            if (!(end.TotalHours > start.TotalHours))
                throw DomainException.CreateFrom(new RestaurantOpeningPeriodEndsBeforeStartFailure());
            Start = start;
            End = end;
        }

        public TimeSpan Start { get; }

        public TimeSpan End { get; }
    }
}
