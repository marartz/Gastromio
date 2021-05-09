using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishInvalidOrderNoFailure : Failure
    {
        public override string ToString()
        {
            return "Gericht hat eine ungültige Reihenfolgenkennzahl";
        }
    }
}
