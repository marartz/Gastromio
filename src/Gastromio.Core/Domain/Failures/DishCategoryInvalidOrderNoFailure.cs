using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishCategoryInvalidOrderNoFailure : Failure
    {
        public override string ToString()
        {
            return "Gerichtkategorie hat eine ungültige Reihenfolgenkennzahl";
        }
    }
}
