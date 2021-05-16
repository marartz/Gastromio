using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantStreetTooLongFailure : Failure
    {
        private readonly int maxLength;

        public RestaurantStreetTooLongFailure(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override string ToString()
        {
            return $"Die Straße des Restaurants ist zu lang (maximum {maxLength} Zeichen)";
        }
    }
}
