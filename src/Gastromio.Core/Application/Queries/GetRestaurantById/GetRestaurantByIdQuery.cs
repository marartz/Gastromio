using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Domain.Model.Restaurant;

namespace Gastromio.Core.Application.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQuery : IQuery<RestaurantDTO>
    {
        public GetRestaurantByIdQuery(string restaurantId, bool onlyActiveRestaurants)
        {
            RestaurantId = restaurantId;
            OnlyActiveRestaurants = onlyActiveRestaurants;
        }

        public string RestaurantId { get; }
        
        public bool OnlyActiveRestaurants { get; }
    }
}
