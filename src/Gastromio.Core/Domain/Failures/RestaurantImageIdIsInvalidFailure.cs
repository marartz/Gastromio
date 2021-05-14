using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantImageIdIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Id des Restaurantbildes ist nicht gültig";
        }
    }
}
