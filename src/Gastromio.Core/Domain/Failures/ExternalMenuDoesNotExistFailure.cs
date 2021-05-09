using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ExternalMenuDoesNotExistFailure : Failure
    {
        public override string ToString()
        {
            return "Die externe Speisekarte existiert nicht";
        }
    }
}
