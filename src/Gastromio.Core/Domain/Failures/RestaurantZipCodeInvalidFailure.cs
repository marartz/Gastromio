using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantZipCodeInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Die Postleitzahl des Restaurants ist nicht gültig: {0}";
        }
    }
}
