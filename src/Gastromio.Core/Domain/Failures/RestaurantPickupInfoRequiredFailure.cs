using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantPickupInfoRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die Informationen zur Abholung für das Restaurant werden benötigt";
        }
    }
}
