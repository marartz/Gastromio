using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishDoesNotExistFailure : Failure
    {
        public override string ToString()
        {
            return "Dish does not exists";
        }
    }
}
