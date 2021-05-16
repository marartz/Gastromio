using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantOpeningPeriodEndsBeforeStartFailure : Failure
    {
        public override string ToString()
        {
            return "Das Ende der Öffnungsperiode muss nach dem Start liegen";
        }
    }
}
