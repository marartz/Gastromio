using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class NoRestaurantDeliveryInfosSpecifiedFailure : Failure
    {
        public override string ToString()
        {
            return "Keine Informationen über die Lieferung spezifiziert";
        }
    }
}
