using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantCityTooLongFailure : Failure
    {
        public override string ToString()
        {
            return "Die Stadt des Restaurants ist zu lang (maximum {0} Zeichen)";
        }
    }
}
