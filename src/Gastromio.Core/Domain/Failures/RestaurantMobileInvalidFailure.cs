using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantMobileInvalidFailure : Failure
    {
        private readonly string mobile;

        public RestaurantMobileInvalidFailure(string mobile)
        {
            this.mobile = mobile;
        }

        public override string ToString()
        {
            return $"Die Mobilnummer des Restaurants ist nicht gültig: {mobile}";
        }
    }
}
