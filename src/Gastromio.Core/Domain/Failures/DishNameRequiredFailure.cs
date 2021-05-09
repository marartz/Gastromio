using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishNameRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Name des Gerichts ist ein Pflichtfeld";
        }
    }
}
