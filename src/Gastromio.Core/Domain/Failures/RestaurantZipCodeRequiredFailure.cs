using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantZipCodeRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die Postleitzahl des Restaurants wird benötigt";
        }
    }
}
