using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantEmailRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die E-Mail-Addresse des Restaurants wird benötigt";
        }
    }
}
