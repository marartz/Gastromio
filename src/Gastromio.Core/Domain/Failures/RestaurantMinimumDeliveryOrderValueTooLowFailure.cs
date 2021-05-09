using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantMinimumDeliveryOrderValueTooLowFailure : Failure
    {
        public override string ToString()
        {
            return "Der Mindestbestellwert für Lieferung ist negativ";
        }
    }
}
