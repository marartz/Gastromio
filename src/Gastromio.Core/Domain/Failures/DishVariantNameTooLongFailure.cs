using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishVariantNameTooLongFailure : Failure
    {
        private readonly int maxLength;

        public DishVariantNameTooLongFailure(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override string ToString()
        {
            return $"Der Name der Variante ist zu lang (maximum {maxLength} Zeichen)";
        }
    }
}
