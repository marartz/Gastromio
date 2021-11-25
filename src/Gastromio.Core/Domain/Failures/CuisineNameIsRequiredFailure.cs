using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class CuisineNameIsRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Name der Cuisine ist ein Pflichtfeld";
        }
    }
}
