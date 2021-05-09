using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class UserAlreadyExistsFailure : Failure
    {
        public override string ToString()
        {
            return "Benutzer existiert bereits";
        }
    }
}
