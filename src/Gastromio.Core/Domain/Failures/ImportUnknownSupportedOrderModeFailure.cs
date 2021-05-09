using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ImportUnknownSupportedOrderModeFailure : Failure
    {
        private readonly string supportedOrderMode;

        public ImportUnknownSupportedOrderModeFailure(string supportedOrderMode)
        {
            this.supportedOrderMode = supportedOrderMode;
        }

        public override string ToString()
        {
            return $"Der angegebene unterstützte Bestellmodus ist nicht bekannt: {supportedOrderMode}";
        }
    }
}
