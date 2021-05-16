using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantMinimumPickupOrderValueTooHighFailure : Failure
    {
        public override string ToString()
        {
            return "Der Mindestbestellwert für Abholung ist zu groß (max. 50€)";
        }
    }
}
