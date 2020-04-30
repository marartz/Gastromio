using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantImage
{
    public class ChangeRestaurantImageCommand : ICommand
    {
        public ChangeRestaurantImageCommand(RestaurantId restaurantId, byte[] image)
        {
            RestaurantId = restaurantId;
            Image = image;
        }

        public RestaurantId RestaurantId { get; }
        public byte[] Image { get; }
    }
}
