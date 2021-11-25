using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishDescriptionTooLongFailure : Failure
    {
        private readonly int maxLength;

        public DishDescriptionTooLongFailure(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override string ToString()
        {
            return $"Die Beschreibung des Gerichts ist zu lang (maximum {maxLength} Zeichen)";
        }
    }
}
