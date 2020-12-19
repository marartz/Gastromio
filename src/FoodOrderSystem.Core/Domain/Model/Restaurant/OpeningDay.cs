using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoodOrderSystem.Core.Common;

namespace FoodOrderSystem.Core.Domain.Model.Restaurant
{
    public abstract class OpeningDay
    {
        private readonly List<OpeningPeriod> openingPeriods;
        
        protected OpeningDay(IEnumerable<OpeningPeriod> openingPeriods)
        {
            this.openingPeriods = openingPeriods?.ToList() ?? new List<OpeningPeriod>();
        }

        public IReadOnlyCollection<OpeningPeriod> OpeningPeriods =>
            new ReadOnlyCollection<OpeningPeriod>(openingPeriods);

        public OpeningPeriod FindPeriodAtTime(TimeSpan time)
        {
            return openingPeriods.SingleOrDefault(en => en.Start <= time && time <= en.End);
        }
        
        public Result<IEnumerable<OpeningPeriod>> AddPeriod(OpeningPeriod openingPeriod)
        {
            if (OpeningPeriods.Any(en => en.Start == openingPeriod.Start && en.End == openingPeriod.End))
                return SuccessResult<IEnumerable<OpeningPeriod>>.Create(OpeningPeriods.AsEnumerable());

            if (openingPeriod.Start.TotalHours < OpeningPeriod.EarliestOpeningTime)
                return FailureResult<IEnumerable<OpeningPeriod>>.Create(FailureResultCode.RestaurantOpeningPeriodBeginsTooEarly);

            if (!(openingPeriod.End.TotalHours > openingPeriod.Start.TotalHours))
                return FailureResult<IEnumerable<OpeningPeriod>>.Create(FailureResultCode.RestaurantOpeningPeriodEndsBeforeStart);

            var anyOverlapping = OpeningPeriods.Any(period => PeriodsOverlapping(openingPeriod, period));

            if (anyOverlapping)
                return FailureResult<IEnumerable<OpeningPeriod>>.Create(FailureResultCode.RestaurantOpeningPeriodIntersects);

            var changedOpeningPeriods = new List<OpeningPeriod>();
            changedOpeningPeriods.AddRange(OpeningPeriods);
            changedOpeningPeriods.Add(openingPeriod);

            return SuccessResult<IEnumerable<OpeningPeriod>>.Create(changedOpeningPeriods);
        }

        public IEnumerable<OpeningPeriod> RemovePeriod(TimeSpan start)
        {
            return openingPeriods.Where(en => en.Start != start);
        }

        private static bool PeriodsOverlapping(OpeningPeriod x, OpeningPeriod y)
        {
            return x.Start.TotalHours <= y.End.TotalHours && y.Start.TotalHours <= x.End.TotalHours;
        }

    }
}