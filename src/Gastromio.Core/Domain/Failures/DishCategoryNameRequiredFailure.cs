using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishCategoryNameRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Name der Kategorie ist ein Pflichtfeld";
        }
    }
}
