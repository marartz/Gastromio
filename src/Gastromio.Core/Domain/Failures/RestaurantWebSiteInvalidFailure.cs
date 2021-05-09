using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantWebSiteInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Die Webseite des Restaurants ist nicht gültig: {0}";
        }
    }
}
