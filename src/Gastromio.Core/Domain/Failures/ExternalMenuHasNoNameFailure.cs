using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ExternalMenuHasNoNameFailure : Failure
    {
        public override string ToString()
        {
            return "Für die externe Speisekarte ist kein Name angegeben";
        }
    }
}
