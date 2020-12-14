using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.DTOs
{
    public class DeviatingOpeningDayDTO
    {
        public DeviatingOpeningDayDTO(DateTime date, IEnumerable<OpeningPeriodDTO> openingPeriods)
        {
            Date = date;
            OpeningPeriods =
                new ReadOnlyCollection<OpeningPeriodDTO>(openingPeriods?.ToList() ?? new List<OpeningPeriodDTO>());
        }

        public DeviatingOpeningDayDTO(DateTime date, IEnumerable<DeviatingOpeningPeriod> openingPeriods)
        {
            Date = date;
            OpeningPeriods = new ReadOnlyCollection<OpeningPeriodDTO>(
                openingPeriods
                    ?.Select(en => new OpeningPeriodDTO(en))
                    .ToList() ?? new List<OpeningPeriodDTO>());
        }

        public DateTime Date { get; }

        public IReadOnlyCollection<OpeningPeriodDTO> OpeningPeriods { get; }
    }
}