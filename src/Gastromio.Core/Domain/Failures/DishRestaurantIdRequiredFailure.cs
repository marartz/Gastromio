using Gastromio.Core.Common;

namespace Gastromio.Core.Domain.Failures
{
    public class DishRestaurantIdRequiredFailure : Failure
    {
        public override string ToString()
        {
            return "Restaurant-ID des Gerichts ist ein Pflichtfeld";
        }
    }
}
