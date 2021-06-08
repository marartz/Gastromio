using System.Collections.Generic;
using System.Linq;
using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class UserIsRestaurantAdminFailure : Failure
    {
        private readonly IList<string> restaurantNames;

        public UserIsRestaurantAdminFailure(IEnumerable<string> restaurantInfos)
        {
            restaurantNames = restaurantInfos.ToList();
        }

        public override string ToString()
        {
            return
                $"Der Benutzer kann nicht gelöscht werden, da er noch als Restaurantadministrator eingetragen ist (Restaurant(s): {string.Join(", ", restaurantNames)}";
        }
    }
}
