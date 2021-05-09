using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class OrderIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Die Bestelldaten sind nicht gültig";
        }
    }
}
