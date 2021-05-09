using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class CannotRemoveCurrentUserFromRestaurantAdminsFailure : Failure
    {
        public override string ToString()
        {
            return "Sie können nicht den gerade angemeldeten Benutzer aus den Administratoren des Restaurants löschen";
        }
    }
}
