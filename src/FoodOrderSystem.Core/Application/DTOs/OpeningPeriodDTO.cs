using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.DTOs
{
    public class OpeningPeriodDTO
    {
        public OpeningPeriodDTO(int dayOfWeek, int start, int end)
        {
            DayOfWeek = dayOfWeek;
            Start = start;
            End = end;
        }

        public OpeningPeriodDTO(OpeningPeriod openingPeriod)
        {
            DayOfWeek = openingPeriod.DayOfWeek;
            Start = (int)openingPeriod.Start.TotalMinutes;
            End = (int) openingPeriod.End.TotalMinutes;
        }
        
        public int DayOfWeek { get; }

        public int Start { get; }
        
        public int End { get; }
    }
}