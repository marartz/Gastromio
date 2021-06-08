using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantAddressRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die Addresse des Restaurants wird benötigt";
        }
    }
}
