using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ImportOpeningPeriodIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Die angegebenen Öffnungszeiten sind ungültig: {0}";
        }
    }
}
