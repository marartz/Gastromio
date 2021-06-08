using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class NoRestaurantPickupInfosSpecifiedFailure : Failure
    {
        public override string ToString()
        {
            return "Keine Informationen über die Abholung spezifiziert";
        }
    }
}
