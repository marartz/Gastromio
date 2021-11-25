using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class PasswordIsNotValidFailure : Failure
    {
        public override string ToString()
        {
            return "Das Passwort ist nicht komplex genug (mind. ein Kleinbuchstabe, ein Großbuchstabe, eine Ziffer und ein Zeichen aus '!@#$%^&')";
        }
    }
}
