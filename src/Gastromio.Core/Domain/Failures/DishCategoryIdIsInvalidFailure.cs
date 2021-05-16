using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishCategoryIdIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Id der Gerichtskategorie ist nicht gültig";
        }
    }
}
