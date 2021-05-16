using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantAverageDeliveryTimeTooHighFailure : Failure
    {
        public override string ToString()
        {
            return "Die durchschnittliche Zeit bis zur Lieferung ist zu groß (max. 120 Minuten)";
        }
    }
}
