using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishCategoryDoesNotExistFailure : Failure
    {
        public override string ToString()
        {
            return "Die Gerichtkategorie ist nicht vorhanden";
        }
    }
}
