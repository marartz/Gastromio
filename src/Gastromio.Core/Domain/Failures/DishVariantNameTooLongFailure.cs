using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishVariantNameTooLongFailure : Failure
    {
        public override string ToString()
        {
            return "Der Name der Variante ist zu lang (maximum {0} Zeichen)";
        }
    }
}
