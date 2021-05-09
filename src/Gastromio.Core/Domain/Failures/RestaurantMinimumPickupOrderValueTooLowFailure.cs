using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantMinimumPickupOrderValueTooLowFailure : Failure
    {
        public override string ToString()
        {
            return "Der Mindestbestellwert für Abholung ist negativ";
        }
    }
}
