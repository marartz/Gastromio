using System.Collections.Generic;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class DeviatingOpeningDayModel
    {
        public DateModel Date { get; set; }

        public List<DeviatingOpeningPeriodModel> OpeningPeriods { get; set; }
    }
}