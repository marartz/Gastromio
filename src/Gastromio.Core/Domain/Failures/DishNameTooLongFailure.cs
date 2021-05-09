using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishNameTooLongFailure : Failure
    {
        private readonly int maxLength;

        public DishNameTooLongFailure(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public override string ToString()
        {
            return $"Der Name des Gerichts ist zu lang (maximum {maxLength} Zeichen)";
        }
    }
}
