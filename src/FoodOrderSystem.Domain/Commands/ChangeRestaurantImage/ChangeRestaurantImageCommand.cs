using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantImage
{
    public class ChangeRestaurantImageCommand : ICommand<bool>
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
