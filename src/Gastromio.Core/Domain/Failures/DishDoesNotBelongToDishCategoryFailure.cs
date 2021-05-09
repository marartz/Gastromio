using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishDoesNotBelongToDishCategoryFailure : Failure
    {
        public override string ToString()
        {
            return "Gericht gehört nicht zur Gerichtkategorie";
        }
    }
}
