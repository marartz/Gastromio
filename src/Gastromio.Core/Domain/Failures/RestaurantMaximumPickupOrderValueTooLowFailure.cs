using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantMaximumPickupOrderValueTooLowFailure : Failure
    {
        public override string ToString()
        {
            return "Der Höchstbestellwert für Abholung ist negativ";
        }
    }
}
