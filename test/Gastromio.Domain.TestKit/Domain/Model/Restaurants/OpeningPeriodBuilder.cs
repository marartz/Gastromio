using System;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Domain.TestKit.Common;

namespace Gastromio.Domain.TestKit.Domain.Model.Restaurants
{
    public class OpeningPeriodBuilder : TestObjectBuilderBase<OpeningPeriod>
    {
        protected override void AddDefaultConstraints()
        {
            // AddConstructorParameterAction((bag, context) =>
            // {
            //     var random = new Random();
            //
            //     if (bag.TryGet<TimeSpan>("start", out var start))
            //     {
            //         if (bag.TryGet<TimeSpan>("end", out var end))
            //             return;
            //
            //         var maxDurationInHours = OpeningPeriod.EarliestOpeningTime + 24 - start.TotalHours;
            //         if (maxDurationInHours < 0)
            //             maxDurationInHours = 0;
            //         var duration = TimeSpan.FromHours(maxDurationInHours * random.NextDouble());
            //
            //         end = start.Add(duration);
            //         bag.Set("end", end);
            //     }
            //     else
            //     {
            //         if (bag.TryGet<TimeSpan>("end", out var end))
            //         {
            //             var maxDurationInHours = end.TotalHours - OpeningPeriod.EarliestOpeningTime;
            //             if (maxDurationInHours < 0)
            //                 maxDurationInHours = 0;
            //             var duration = TimeSpan.FromHours(maxDurationInHours * random.NextDouble());
            //
            //             start = end.Add(-duration);
            //             bag.Set("start", start);
            //         }
            //         else
            //         {
            //             start = TimeSpan.FromHours(OpeningPeriod.EarliestOpeningTime + 10 * random.NextDouble());
            //             bag.Set("start", start);
            //
            //             var maxDurationInHours = OpeningPeriod.EarliestOpeningTime + 24 - start.TotalHours;
            //             var duration = TimeSpan.FromHours(maxDurationInHours * random.NextDouble());
            //
            //             end = start.Add(duration);
            //             bag.Set("end", end);
            //         }
            //     }
            // });
        }

        public OpeningPeriodBuilder WithStart(TimeSpan start)
        {
            WithConstantConstructorArgumentFor("start", start);
            return this;
        }

        public OpeningPeriodBuilder WithEnd(TimeSpan end)
        {
            WithConstantConstructorArgumentFor("end", end);
            return this;
        }

    }
}
