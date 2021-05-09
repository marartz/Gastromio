using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishCategoryAlreadyExistsFailure : Failure
    {
        public override string ToString()
        {
            return "Es gibt bereits eine Gerichtkategorie mit gleichem Namen";
        }
    }
}
