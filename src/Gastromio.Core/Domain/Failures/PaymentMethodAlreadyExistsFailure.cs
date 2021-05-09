using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class PaymentMethodAlreadyExistsFailure : Failure
    {
        public override string ToString()
        {
            return "Zahlungsmethode existiert bereits";
        }
    }
}
