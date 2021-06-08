using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantDeviatingOpeningDayHasStillOpenPeriodsFailure : Failure
    {
        public override string ToString()
        {
            return "Der abweichende Öffnungstag hat noch registrierte Öffnungszeiten";
        }
    }
}
