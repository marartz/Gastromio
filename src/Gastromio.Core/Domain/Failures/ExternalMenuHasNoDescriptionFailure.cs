using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ExternalMenuHasNoDescriptionFailure : Failure
    {
        public override string ToString()
        {
            return "Für die externe Speisekarte ist keine Beschreibung angegeben";
        }
    }
}
