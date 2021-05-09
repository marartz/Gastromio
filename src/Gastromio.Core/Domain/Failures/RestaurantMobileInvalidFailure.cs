using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantMobileInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Die Mobilnummer des Restaurants ist nicht gültig: {0}";
        }
    }
}
