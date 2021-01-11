using Gastromio.Core.Domain.Model.Restaurant;

namespace Gastromio.Core.Application.Commands.ChangeRestaurantImage
{
    public class ChangeRestaurantImageCommand : ICommand<bool>
    {
        public ChangeRestaurantImageCommand(RestaurantId restaurantId, string type, byte[] image)
        {
            RestaurantId = restaurantId;
            Type = type;
            Image = image;
        }

        public RestaurantId RestaurantId { get; }
        public string Type { get; }
        public byte[] Image { get; }
    }
}
