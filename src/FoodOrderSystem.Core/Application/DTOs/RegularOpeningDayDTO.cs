using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.DTOs
{
    public class RegularOpeningDayDTO
    {
        public RegularOpeningDayDTO(int dayOfWeek, IEnumerable<OpeningPeriodDTO> openingPeriods)
        {
            DayOfWeek = dayOfWeek;
            OpeningPeriods =
                new ReadOnlyCollection<OpeningPeriodDTO>(openingPeriods?.ToList() ?? new List<OpeningPeriodDTO>());
        }

        public RegularOpeningDayDTO(int dayOfWeek, IEnumerable<OpeningPeriod> openingPeriods)
        {
            DayOfWeek = dayOfWeek;
            OpeningPeriods = new ReadOnlyCollection<OpeningPeriodDTO>(
                openingPeriods
                    ?.Select(en => new OpeningPeriodDTO(en))
                    .ToList() ?? new List<OpeningPeriodDTO>());
        }

        public int DayOfWeek { get; }

        public IReadOnlyCollection<OpeningPeriodDTO> OpeningPeriods { get; }
    }
}