using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantDoesNotExistFailure : Failure
    {
        public override string ToString()
        {
            return "Restaurant existiert nicht";
        }
    }
}
