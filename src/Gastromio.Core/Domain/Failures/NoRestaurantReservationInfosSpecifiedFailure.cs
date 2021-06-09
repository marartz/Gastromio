using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class NoRestaurantReservationInfosSpecifiedFailure : Failure
    {
        public override string ToString()
        {
            return "Keine Informationen über die Reservierung spezifiziert";
        }
    }
}
