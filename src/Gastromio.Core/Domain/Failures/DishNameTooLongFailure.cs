using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishNameTooLongFailure : Failure
    {
        public override string ToString()
        {
            return "Der Name des Gerichts ist zu lang (maximum {0} Zeichen)";
        }
    }
}
