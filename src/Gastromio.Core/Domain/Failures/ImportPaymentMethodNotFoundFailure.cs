using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ImportPaymentMethodNotFoundFailure : Failure
    {
        public override string ToString()
        {
            return "Die angegebene Zahlungsmethode ist nicht bekannt: {0}";
        }
    }
}
