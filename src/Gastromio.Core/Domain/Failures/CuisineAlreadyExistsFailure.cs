using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class CuisineAlreadyExistsFailure : Failure
    {
        public override string ToString()
        {
            return "Cuisine existiert bereits";
        }
    }
}
