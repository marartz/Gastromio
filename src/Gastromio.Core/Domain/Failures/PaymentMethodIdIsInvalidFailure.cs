using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class PaymentMethodIdIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Id der Zahlungsmethode ist nicht gültig";
        }
    }
}
