using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantEmailInvalidFailure : Failure
    {
        private readonly string email;

        public RestaurantEmailInvalidFailure(string email)
        {
            this.email = email;
        }

        public override string ToString()
        {
            return $"Die E-Mail-Addresse des Restaurants ist nicht gültig: {email}";
        }
    }
}
