using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantResponsiblePersonRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die verantwortliche Person des Restaurants wird benötigt";
        }
    }
}
