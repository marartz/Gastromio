using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantImageTypeRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Der Typ der Bilddatei wird benötigt";
        }
    }
}
