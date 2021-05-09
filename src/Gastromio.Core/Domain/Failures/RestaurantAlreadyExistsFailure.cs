using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantAlreadyExistsFailure : Failure
    {
        public override string ToString()
        {
            return "Gleichnamiges Restaurant existiert bereits";
        }
    }
}
