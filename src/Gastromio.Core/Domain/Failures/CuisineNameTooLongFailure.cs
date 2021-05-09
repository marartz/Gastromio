using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class CuisineNameTooLongFailure : Failure
    {
        public override string ToString()
        {
            return "Der Name der Cuisine ist zu lang (maximum {0} Zeichen)";
        }
    }
}
