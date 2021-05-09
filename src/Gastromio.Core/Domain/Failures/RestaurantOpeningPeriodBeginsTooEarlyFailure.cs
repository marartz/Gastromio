using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantOpeningPeriodBeginsTooEarlyFailure : Failure
    {
        public override string ToString()
        {
            return "Die Öffnungsperiode darf nicht vor 4 Uhr morgens beginnen";
        }
    }
}
