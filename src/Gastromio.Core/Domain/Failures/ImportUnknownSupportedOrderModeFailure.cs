using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ImportUnknownSupportedOrderModeFailure : Failure
    {
        public override string ToString()
        {
            return "Der angegebene unterstützte Bestellmodus ist nicht bekannt: {0}";
        }
    }
}
