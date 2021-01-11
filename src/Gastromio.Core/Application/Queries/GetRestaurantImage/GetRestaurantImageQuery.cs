using Gastromio.Core.Domain.Model.Restaurant;
using Gastromio.Core.Domain.Model.RestaurantImage;

namespace Gastromio.Core.Application.Queries.GetRestaurantImage
{
    public class GetRestaurantImageQuery : IQuery<RestaurantImage>
    {
        public GetRestaurantImageQuery(RestaurantId restaurantId, string type)
        {
            RestaurantId = restaurantId;
            Type = type;
        }
        
        public RestaurantId RestaurantId { get; }

        public string Type { get; }
    }
}