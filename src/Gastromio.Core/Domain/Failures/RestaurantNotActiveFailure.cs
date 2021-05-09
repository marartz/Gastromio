using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantNotActiveFailure : Failure
    {
        public override string ToString()
        {
            return "Restaurant ist nicht aktiv";
        }
    }
}
