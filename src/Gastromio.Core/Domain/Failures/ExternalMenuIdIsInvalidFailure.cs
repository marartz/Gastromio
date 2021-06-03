using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class ExternalMenuIdIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Id der externen Speisekarte ist nicht gültig";
        }
    }
}
