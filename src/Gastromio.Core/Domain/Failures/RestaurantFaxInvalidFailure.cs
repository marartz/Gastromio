using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantFaxInvalidFailure : Failure
    {
        private readonly string fax;

        public RestaurantFaxInvalidFailure(string fax)
        {
            this.fax = fax;
        }

        public override string ToString()
        {
            return $"Die Faxnummer des Restaurants ist nicht gültig: {fax}";
        }
    }
}
