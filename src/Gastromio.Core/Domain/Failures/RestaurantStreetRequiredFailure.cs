using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantStreetRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die Straße des Restaurants wird benötigt";
        }
    }
}
