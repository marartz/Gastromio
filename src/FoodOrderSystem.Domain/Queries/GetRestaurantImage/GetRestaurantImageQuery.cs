using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.RestaurantImage;

namespace FoodOrderSystem.Domain.Queries.GetRestaurantImage
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