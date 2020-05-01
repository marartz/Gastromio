using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantContactDetails
{
    public class ChangeRestaurantContactDetailsCommand : ICommand<bool>
    {
        public ChangeRestaurantContactDetailsCommand(RestaurantId restaurantId, string phone, string webSite, string imprint, string orderEmailAddress)
        {
            RestaurantId = restaurantId;
            Phone = phone;
            WebSite = webSite;
            Imprint = imprint;
            OrderEmailAddress = orderEmailAddress;
        }

        public RestaurantId RestaurantId { get; }
        public string Phone { get; }
        public string WebSite { get; }
        public string Imprint { get; }
        public string OrderEmailAddress { get; }
    }
}
