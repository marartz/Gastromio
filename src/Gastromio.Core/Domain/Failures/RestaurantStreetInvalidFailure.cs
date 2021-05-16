using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantStreetInvalidFailure : Failure
    {
        private readonly string street;

        public RestaurantStreetInvalidFailure(string street)
        {
            this.street = street;
        }

        public override string ToString()
        {
            return $"Die Straße des Restaurants ist nicht gültig: {street}";
        }
    }
}
