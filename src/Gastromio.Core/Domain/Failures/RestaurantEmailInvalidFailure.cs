using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantEmailInvalidFailure : Failure
    {
        public override string ToString()
        {
            return"Die E-Mail-Addresse des Restaurants ist nicht gültig: {0}";
        }
    }
}
