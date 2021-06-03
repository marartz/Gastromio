using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantAveragePickupTimeTooLowFailure : Failure
    {
        public override string ToString()
        {
            return "Die durchschnittliche Zeit bis zur Abholung ist zu klein (mind. 5 Minuten)";
        }
    }
}
