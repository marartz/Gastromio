using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantAveragePickupTimeTooHighFailure : Failure
    {
        public override string ToString()
        {
            return "Die durchschnittliche Zeit bis zur Abholung ist zu groß (max. 120 Minuten)";
        }
    }
}
