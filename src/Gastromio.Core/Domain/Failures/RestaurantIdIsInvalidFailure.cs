using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantIdIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Id des Restaurants ist nicht gültig";
        }
    }
}
