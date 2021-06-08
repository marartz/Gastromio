using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishCategoryNameTooLongFailure : Failure
    {
        private readonly int maxLength;

        public DishCategoryNameTooLongFailure(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override string ToString()
        {
            return $"Der Name der Kategorie ist zu lang (maximum {maxLength} Zeichen)";
        }
    }
}
