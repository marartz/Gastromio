using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantNameRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Name des Restaurants ist ein Pflichtfeld";
        }
    }
}
