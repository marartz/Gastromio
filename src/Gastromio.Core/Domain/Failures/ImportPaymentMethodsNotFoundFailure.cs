using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ImportPaymentMethodsNotFoundFailure : Failure
    {
        public override string ToString()
        {
            return "Keine Zahlungsmethoden angegeben";
        }
    }
}
