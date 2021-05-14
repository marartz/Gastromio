using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantDeliveryInfoRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die Informationen zur Lieferung für das Restaurant werden benötigt";
        }
    }
}
