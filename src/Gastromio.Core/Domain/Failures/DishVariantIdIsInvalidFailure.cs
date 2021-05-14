using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishVariantIdIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Id der Gerichtsvariante ist nicht gültig";
        }
    }
}
