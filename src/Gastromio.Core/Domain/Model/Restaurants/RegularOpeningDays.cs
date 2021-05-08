using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public class RegularOpeningDays : IReadOnlyCollection<RegularOpeningDay>
    {
        private readonly IReadOnlyDictionary<int, RegularOpeningDay> openingDayDict;

        public RegularOpeningDays(IEnumerable<RegularOpeningDay> openingDays)
        {
            var tempDict = new Dictionary<int, RegularOpeningDay>();
            foreach (var openingDay in openingDays)
            {
                if (tempDict.ContainsKey(openingDay.DayOfWeek))
                    throw new InvalidOperationException("day of week is already registered");
                tempDict.Add(openingDay.DayOfWeek, openingDay);
            }

            openingDayDict = new ReadOnlyDictionary<int, RegularOpeningDay>(tempDict);
        }

        public bool TryGetOpeningDay(int dayOfWeek, out RegularOpeningDay openingDay)
        {
            return openingDayDict.TryGetValue(dayOfWeek, out openingDay);
        }

        public RegularOpeningDays AddOpeningPeriod(int dayOfWeek, OpeningPeriod openingPeriod)
        {
            if (TryGetOpeningDay(dayOfWeek, out var openingDay))
                return Replace(openingDay.AddPeriod(openingPeriod));

            var openingPeriods = new OpeningPeriods(new[] {openingPeriod});
            openingDay = new RegularOpeningDay(dayOfWeek, openingPeriods);
            return Append(openingDay);
        }

        public RegularOpeningDays RemoveOpeningPeriod(int dayOfWeek, TimeSpan start)
        {
            if (!TryGetOpeningDay(dayOfWeek, out var openingDay))
                return this;
            var changedOpeningDay = openingDay.RemovePeriod(start);
            return changedOpeningDay.HasOpeningPeriods
                ? Replace(changedOpeningDay)
                : Remove(dayOfWeek);
        }

        public IEnumerator<RegularOpeningDay> GetEnumerator()
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

        private RegularOpeningDays Append(RegularOpeningDay openingDay)
        {
            var newOpeningDays = openingDayDict
                .Select(en => en.Value)
                .Append(openingDay);
            return new RegularOpeningDays(newOpeningDays);
        }

        private RegularOpeningDays Replace(RegularOpeningDay openingDay)
        {
            var newOpeningDays = openingDayDict
                .Where(en => en.Key != openingDay.DayOfWeek)
                .Select(en => en.Value)
                .Append(openingDay);
            return new RegularOpeningDays(newOpeningDays);
        }

        private RegularOpeningDays Remove(int dayOfWeek)
        {
            var newOpeningDays = openingDayDict
                .Where(en => en.Key != dayOfWeek)
                .Select(en => en.Value);
            return new RegularOpeningDays(newOpeningDays);
        }
    }
}
