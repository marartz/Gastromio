using Gastromio.Core.Domain.Model.Restaurants;

namespace Gastromio.Core.Application.Commands.SetAliasOfRestaurant
{
    public class SetAliasOfRestaurantCommand : ICommand<bool>
    {
        public SetAliasOfRestaurantCommand(RestaurantId restaurantId, string alias)
        {
            RestaurantId = restaurantId;
            Alias = alias;
        }

        public RestaurantId RestaurantId { get; }
        public string Alias { get; }
    }
}
