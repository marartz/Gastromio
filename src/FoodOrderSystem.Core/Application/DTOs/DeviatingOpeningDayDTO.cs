using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.DTOs
{
    public class DeviatingOpeningDayDTO
    {
        public DeviatingOpeningDayDTO(Date date, DeviatingOpeningDayStatus status, IEnumerable<OpeningPeriod> openingPeriods)
        {
            Date = date;
            Status = status.ToModel();
            OpeningPeriods = new ReadOnlyCollection<OpeningPeriodDTO>(
                openingPeriods
                    ?.Select(en => new OpeningPeriodDTO(en))
                    .ToList() ?? new List<OpeningPeriodDTO>());
        }

        public Date Date { get; }
        
        public string Status { get; }

        public IReadOnlyCollection<OpeningPeriodDTO> OpeningPeriods { get; }
    }
}