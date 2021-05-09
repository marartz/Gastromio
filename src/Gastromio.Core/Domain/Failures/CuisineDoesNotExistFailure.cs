using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class CuisineDoesNotExistFailure : Failure
    {
        public override string ToString()
        {
            return "Cuisine existiert nicht";
        }
    }
}
