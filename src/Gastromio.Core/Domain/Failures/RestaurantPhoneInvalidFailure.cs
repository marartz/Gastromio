using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantPhoneInvalidFailure : Failure
    {
        private readonly string phone;

        public RestaurantPhoneInvalidFailure(string phone)
        {
            this.phone = phone;
        }

        public override string ToString()
        {
            return $"Die Telefonnummer des Restaurants ist nicht gültig: {phone}";
        }
    }
}
