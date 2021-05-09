using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantStreetTooLongFailure : Failure
    {
        public override string ToString()
        {
            return "Die Straße des Restaurants ist zu lang (maximum {0} Zeichen)";
        }
    }
}
