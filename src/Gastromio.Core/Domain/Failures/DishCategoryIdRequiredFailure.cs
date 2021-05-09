using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishCategoryIdRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Kategorie-ID des Gerichts ist ein Pflichtfeld";
        }
    }
}
