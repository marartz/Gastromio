using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantZipCodeInvalidFailure : Failure
    {
        private readonly string zipCode;

        public RestaurantZipCodeInvalidFailure(string zipCode)
        {
            this.zipCode = zipCode;
        }

        public override string ToString()
        {
            return $"Die Postleitzahl des Restaurants ist nicht gültig: {zipCode}";
        }
    }
}
