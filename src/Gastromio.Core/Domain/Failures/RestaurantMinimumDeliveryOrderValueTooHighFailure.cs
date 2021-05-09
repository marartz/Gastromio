using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantMinimumDeliveryOrderValueTooHighFailure : Failure
    {
        public override string ToString()
        {
            return "Der Mindestbestellwert für Lieferung ist zu groß (max. 50€)";
        }
    }
}
