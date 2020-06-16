using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Queries.GetRestaurantImage
{
    public class GetRestaurantImageQuery : IQuery<byte[]>
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