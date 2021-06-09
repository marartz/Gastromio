using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class DeviatingOpeningDays : IReadOnlyCollection<DeviatingOpeningDay>
    {
        private readonly IReadOnlyDictionary<Date, DeviatingOpeningDay> openingDayDict;

        public DeviatingOpeningDays(IEnumerable<DeviatingOpeningDay> openingDays)
        {
            var tempDict = new Dictionary<Date, DeviatingOpeningDay>();
            foreach (var openingDay in openingDays)
            {
                if (tempDict.ContainsKey(openingDay.Date))
                    throw DomainException.CreateFrom(new DeviatingOpeningDayIsAlreadyRegisteredFailure(openingDay.Date));
                tempDict.Add(openingDay.Date, openingDay);
            }

            openingDayDict = new ReadOnlyDictionary<Date, DeviatingOpeningDay>(tempDict);
        }

        public bool TryGetOpeningDay(Date date, out DeviatingOpeningDay openingDay)
        {
            return openingDayDict.TryGetValue(date, out openingDay);
        }

        public DeviatingOpeningDays AddOpeningDay(Date date, DeviatingOpeningDayStatus status)
        {
            return TryGetOpeningDay(date, out _)
                ? this
                : Append(new DeviatingOpeningDay(date, status, new OpeningPeriods(Enumerable.Empty<OpeningPeriod>())));
        }

        public DeviatingOpeningDays ChangeOpeningDayStatus(Date date, DeviatingOpeningDayStatus status)
        {
            if (!TryGetOpeningDay(date, out var openingDay))
                throw DomainException.CreateFrom(new RestaurantDeviatingOpeningDayDoesNotExistFailure());
            return Replace(openingDay.ChangeStatus(status));
        }

        public DeviatingOpeningDays RemoveOpeningDay(Date date)
        {
            return TryGetOpeningDay(date, out _)
                ? Remove(date)
                : this;
        }

        public DeviatingOpeningDays AddOpeningPeriod(Date date, OpeningPeriod openingPeriod)
        {
            if (!TryGetOpeningDay(date, out var openingDay))
                throw DomainException.CreateFrom(new RestaurantDeviatingOpeningDayDoesNotExistFailure());
            return Replace(openingDay.AddPeriod(openingPeriod));
        }

        public DeviatingOpeningDays RemoveOpeningPeriod(Date date, TimeSpan start)
        {
            return TryGetOpeningDay(date, out var openingDay)
                ? Replace(openingDay.RemovePeriod(start))
                : this;
        }

        public IEnumerator<DeviatingOpeningDay> GetEnumerator()
        {
            return openingDayDict
                .OrderBy(en => en.Key)
                .Select(en => en.Value)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count
        {
            get { return openingDayDict.Count; }
        }

        private DeviatingOpeningDays Append(DeviatingOpeningDay openingDay)
        {
            var newOpeningDays = openingDayDict
                .Select(en => en.Value)
                .Append(openingDay);
            return new DeviatingOpeningDays(newOpeningDays);
        }

        private DeviatingOpeningDays Replace(DeviatingOpeningDay openingDay)
        {
            var newOpeningDays = openingDayDict
                .Where(en => en.Key != openingDay.Date)
                .Select(en => en.Value)
                .Append(openingDay);
            return new DeviatingOpeningDays(newOpeningDays);
        }

        private DeviatingOpeningDays Remove(Date date)
        {
            var newOpeningDays = openingDayDict
                .Where(en => en.Key != date)
                .Select(en => en.Value);
            return new DeviatingOpeningDays(newOpeningDays);
        }
    }
}
