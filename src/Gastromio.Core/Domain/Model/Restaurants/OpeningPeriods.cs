using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class OpeningPeriods : IReadOnlyCollection<OpeningPeriod>
    {
        private readonly IReadOnlyCollection<OpeningPeriod> openingPeriodList;

        public OpeningPeriods(IEnumerable<OpeningPeriod> openingPeriods)
        {
            var tempList = new List<OpeningPeriod>();
            foreach (var openingPeriod in openingPeriods)
            {
                if (tempList.Any(en => en.Start == openingPeriod.Start && en.End == openingPeriod.End))
                    continue;
                var anyOverlapping = tempList.Any(period => PeriodsOverlapping(openingPeriod, period));
                if (anyOverlapping)
                    throw DomainException.CreateFrom(new RestaurantOpeningPeriodIntersectsFailure());
                tempList.Add(openingPeriod);
            }
            openingPeriodList = new ReadOnlyCollection<OpeningPeriod>(tempList);
        }

        public OpeningPeriod FindPeriodAtTime(TimeSpan time)
        {
            return openingPeriodList.SingleOrDefault(en => en.Start <= time && time <= en.End);
        }

        public OpeningPeriods Add(OpeningPeriod openingPeriod)
        {
            var newOpeningPeriods = openingPeriodList
                .Append(openingPeriod);
            return new OpeningPeriods(newOpeningPeriods);
        }

        public OpeningPeriods Remove(TimeSpan start)
        {
            var newOpeningPeriods = openingPeriodList
                .Where(en => en.Start != start);
            return new OpeningPeriods(newOpeningPeriods);
        }

        public IEnumerator<OpeningPeriod> GetEnumerator()
        {
            return openingPeriodList
                .OrderBy(en => en.Start)
                .ThenBy(en => en.End)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return openingPeriodList.Count; }
        }

        private static bool PeriodsOverlapping(OpeningPeriod x, OpeningPeriod y)
        {
            return x.Start.TotalHours <= y.End.TotalHours && y.Start.TotalHours <= x.End.TotalHours;
        }
    }
}
