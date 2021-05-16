using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantOpeningPeriodIntersectsFailure : Failure
    {
        public override string ToString()
        {
            return "Die Öffnungsperiode überschneidet sich mit einer bereits eingetragenen Öffnungsperiode";
        }
    }
}
