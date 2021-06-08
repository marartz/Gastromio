using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class OrderIdIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Id der Bestellung ist nicht gültig";
        }
    }
}
