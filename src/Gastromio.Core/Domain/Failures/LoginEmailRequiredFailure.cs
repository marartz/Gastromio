using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class LoginEmailRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Die E-Mail-Addresse wird für den Login benötigt";
        }
    }
}
