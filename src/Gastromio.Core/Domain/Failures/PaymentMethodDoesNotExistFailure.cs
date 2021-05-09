using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class PaymentMethodDoesNotExistFailure : Failure
    {
        public override string ToString()
        {
            return "Zahlungsmethode existiert nicht";
        }
    }
}
