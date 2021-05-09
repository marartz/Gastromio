using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantStreetInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Die Straße des Restaurants ist nicht gültig: {0}";
        }
    }
}
