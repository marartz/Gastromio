using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishDescriptionTooLongFailure : Failure
    {
        public override string ToString()
        {
            return "Die Beschreibung des Gerichts ist zu lang (maximum {0} Zeichen)";
        }
    }
}
