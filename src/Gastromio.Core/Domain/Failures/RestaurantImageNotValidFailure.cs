using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class RestaurantImageNotValidFailure : Failure
    {
        public override string ToString()
        {
            return "Die angegebene Bilddatei ist nicht gültig";
        }
    }
}
