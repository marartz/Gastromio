using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class PasswordResetCodeIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Dieser Link ist leider nicht (mehr) gültig, bitte fordere nochmals die Änderung Deines Passworts an";
        }
    }
}
