using System;

namespace Gastromio.Core.Domain.Model.Restaurants
{
    public abstract class OpeningDay<TSpecificOpeningDay>
    {
        protected OpeningDay(OpeningPeriods openingPeriods)
        {
            OpeningPeriods = openingPeriods;
        }

        public OpeningPeriods OpeningPeriods { get; }

        public bool HasOpeningPeriods
        {
            get
            {
                return OpeningPeriods.Count > 0;
            }
        }


        public OpeningPeriod FindPeriodAtTime(TimeSpan time)
        {
            return OpeningPeriods.FindPeriodAtTime(time);
        }

        public TSpecificOpeningDay AddPeriod(OpeningPeriod openingPeriod)
        {
            return CreateSpecificOpeningDayWith(OpeningPeriods.Add(openingPeriod));
        }

        public TSpecificOpeningDay RemovePeriod(TimeSpan start)
        {
            return CreateSpecificOpeningDayWith(OpeningPeriods.Remove(start));
        }

        protected abstract TSpecificOpeningDay CreateSpecificOpeningDayWith(OpeningPeriods openingPeriods);
    }
}
