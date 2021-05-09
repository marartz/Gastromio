using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class LoginPasswordRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Das Passwort wird für den Login benötigt";
        }
    }
}
