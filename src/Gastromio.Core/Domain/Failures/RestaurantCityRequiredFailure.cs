using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantCityRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die Stadt des Restaurants wird benötigt";
        }
    }
}
