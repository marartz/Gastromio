using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantContactInfo
{
    public class ChangeRestaurantContactInfoCommand : ICommand<bool>
    {
        public ChangeRestaurantContactInfoCommand(RestaurantId restaurantId, ContactInfo contactInfo)
        {
            RestaurantId = restaurantId;
            ContactInfo = contactInfo;
        }

        public RestaurantId RestaurantId { get; }
        
        public ContactInfo ContactInfo { get; }
    }
}
