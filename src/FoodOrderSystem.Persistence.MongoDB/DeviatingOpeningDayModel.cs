using System.Collections.Generic;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class DeviatingOpeningDayModel
    {
        public DateModel Date { get; set; }
        
        public string Status { get; set; } // "open", "closed", "fully-booked"

        public List<DeviatingOpeningPeriodModel> OpeningPeriods { get; set; }
    }
}