using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantDeliveryCostsTooHighFailure : Failure
    {
        public override string ToString()
        {
            return "Die Lieferkosten sind zu groß (max. 10€)";
        }
    }
}
