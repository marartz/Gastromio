using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantMaximumDeliveryOrderValueTooLowFailure : Failure
    {
        public override string ToString()
        {
            return "Der Höchstbestellwert für Lieferung ist negativ";
        }
    }
}
