using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishProductInfoTooLongFailure : Failure
    {
        private readonly int maxLength;

        public DishProductInfoTooLongFailure(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override string ToString()
        {
            return $"Die Produktinformation des Gerichts ist zu lang (maximum {maxLength} Zeichen)";
        }
    }
}
