using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ImportOrderTypeIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Die angegebene Bestellart ist ungültig: {0}";
        }
    }
}
