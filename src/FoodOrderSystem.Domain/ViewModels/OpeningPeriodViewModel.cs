using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.ViewModels
{
    public class OpeningPeriodViewModel
    {
        public int DayOfWeek { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public static OpeningPeriodViewModel FromOpeningPeriod(OpeningPeriod openingPeriod)
        {
            return new OpeningPeriodViewModel
            {
                DayOfWeek = openingPeriod.DayOfWeek,
                Start = (int)openingPeriod.Start.TotalMinutes,
                End = (int)openingPeriod.End.TotalMinutes
            };
        }
    }
}
