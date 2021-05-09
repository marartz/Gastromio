using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class WrongCredentialsFailure : Failure
    {
        public override string ToString()
        {
            return "Emailadresse und/oder Passwort ist nicht korrekt";
        }
    }
}
