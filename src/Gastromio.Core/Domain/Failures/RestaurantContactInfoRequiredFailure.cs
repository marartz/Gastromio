using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantContactInfoRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die Kontaktinformationen des Restaurants werden benötigt";
        }
    }
}
