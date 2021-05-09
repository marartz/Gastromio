using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantFaxInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Die Faxnummer des Restaurants ist nicht gültig: {0}";
        }
    }
}
