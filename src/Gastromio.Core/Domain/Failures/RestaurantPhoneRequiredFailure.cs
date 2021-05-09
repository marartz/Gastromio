using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantPhoneRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die Telefonnummer des Restaurants wird benötigt";
        }
    }
}
