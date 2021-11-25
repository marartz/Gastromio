using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ImportPaymentMethodNotFoundFailure : Failure
    {
        private readonly string paymentMethod;

        public ImportPaymentMethodNotFoundFailure(string paymentMethod)
        {
            this.paymentMethod = paymentMethod;
        }

        public override string ToString()
        {
            return $"Die angegebene Zahlungsmethode ist nicht bekannt: {paymentMethod}";
        }
    }
}
