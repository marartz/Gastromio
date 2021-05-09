using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishProductInfoTooLongFailure : Failure
    {
        public override string ToString()
        {
            return "Die Produktinformation des Gerichts ist zu lang (maximum {0} Zeichen)";
        }
    }
}
