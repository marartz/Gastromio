using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantPhoneInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Die Telefonnummer des Restaurants ist nicht gültig: {0}";
        }
    }
}
