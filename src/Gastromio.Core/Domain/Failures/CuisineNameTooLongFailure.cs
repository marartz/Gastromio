using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class CuisineNameTooLongFailure : Failure
    {
        private readonly int maxLength;

        public CuisineNameTooLongFailure(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override string ToString()
        {
            return $"Der Name der Cuisine ist zu lang (maximum {maxLength} Zeichen)";
        }
    }
}
