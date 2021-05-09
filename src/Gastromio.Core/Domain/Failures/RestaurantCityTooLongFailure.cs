using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantCityTooLongFailure : Failure
    {
        private readonly int maxLength;

        public RestaurantCityTooLongFailure(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override string ToString()
        {
            return $"Die Stadt des Restaurants ist zu lang (maximum {maxLength} Zeichen)";
        }
    }
}
