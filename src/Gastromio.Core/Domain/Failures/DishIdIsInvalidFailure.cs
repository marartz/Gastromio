using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishIdIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Id des Gerichts ist nicht gültig";
        }
    }
}
