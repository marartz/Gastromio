using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class CuisineIdIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Id der Cuisine ist nicht gültig";
        }
    }
}
