using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantReservationInfoRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die Informationen zur Reservierung für das Restaurant werden benötigt";
        }
    }
}
