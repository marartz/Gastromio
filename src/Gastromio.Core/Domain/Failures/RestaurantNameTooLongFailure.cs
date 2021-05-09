using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantNameTooLongFailure : Failure
    {
        private readonly int maxLength;

        public RestaurantNameTooLongFailure(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override string ToString()
        {
            return $"Der Name des Restaurants ist zu lang (maximum {maxLength} Zeichen)";
        }
    }
}
