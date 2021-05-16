using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantDeliveryCostsTooLowFailure : Failure
    {
        public override string ToString()
        {
            return "Die Lieferkosten sind negativ";
        }
    }
}
