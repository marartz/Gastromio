using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.DTOs
{
    public class DeviatingOpeningDayDTO
    {
        public DeviatingOpeningDayDTO(Date date, IEnumerable<OpeningPeriodDTO> openingPeriods)
        {
            Date = date;
            OpeningPeriods =
                new ReadOnlyCollection<OpeningPeriodDTO>(openingPeriods?.ToList() ?? new List<OpeningPeriodDTO>());
        }

        public DeviatingOpeningDayDTO(Date date, IEnumerable<DeviatingOpeningPeriod> openingPeriods)
        {
            Date = date;
            OpeningPeriods = new ReadOnlyCollection<OpeningPeriodDTO>(
                openingPeriods
                    ?.Select(en => new OpeningPeriodDTO(en))
                    .ToList() ?? new List<OpeningPeriodDTO>());
        }

        public Date Date { get; }

        public IReadOnlyCollection<OpeningPeriodDTO> OpeningPeriods { get; }
    }
}