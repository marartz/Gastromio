using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class SessionExpiredFailure : Failure
    {
        public override string ToString()
        {
            return "Du bist nicht angemeldet";
        }
    }
}
