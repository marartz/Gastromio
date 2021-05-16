using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantAverageDeliveryTimeTooLowFailure : Failure
    {
        public override string ToString()
        {
            return "Die durchschnittliche Zeit bis zur Lieferung ist zu klein (mind. 5 Minuten)";
        }
    }
}
