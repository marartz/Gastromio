using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishCategoryNameTooLongFailure : Failure
    {
        public override string ToString()
        {
            return "Der Name der Kategorie ist zu lang (maximum {0} Zeichen)";
        }
    }
}
