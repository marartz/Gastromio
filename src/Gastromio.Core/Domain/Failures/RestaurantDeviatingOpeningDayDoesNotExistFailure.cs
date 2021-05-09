using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantDeviatingOpeningDayDoesNotExistFailure : Failure
    {
        public override string ToString()
        {
            return "Der abweichende Öffnungstag ist nicht bekannt";
        }
    }
}
