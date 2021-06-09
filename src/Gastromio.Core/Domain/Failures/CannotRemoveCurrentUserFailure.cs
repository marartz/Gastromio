using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class CannotRemoveCurrentUserFailure : Failure
    {
        public override string ToString()
        {
            return "Sie können nicht den gerade angemeldeten Benutzer löschen";
        }
    }
}
