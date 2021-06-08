using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class UserIdIsInvalidFailure : Failure
    {
        public override string ToString()
        {
            return "Id des Benutzers ist nicht gültig";
        }
    }
}
