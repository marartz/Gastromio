using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.DTOs
{
    public class OpeningPeriodDTO
    {
        public OpeningPeriodDTO(int start, int end)
        {
            Start = start;
            End = end;
        }

        public OpeningPeriodDTO(RegularOpeningPeriod regularOpeningPeriod)
        {
            Start = (int) regularOpeningPeriod.Start.TotalMinutes;
            End = (int) regularOpeningPeriod.End.TotalMinutes;
        }
        
        public OpeningPeriodDTO(DeviatingOpeningPeriod deviatingOpeningPeriod)
        {
            Start = (int) deviatingOpeningPeriod.Start.TotalMinutes;
            End = (int) deviatingOpeningPeriod.End.TotalMinutes;
        }
        
        public int Start { get; }
        
        public int End { get; }
        
    }
}