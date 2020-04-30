using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantContactDetails
{
    public class ChangeRestaurantContactDetailsCommand : ICommand
    {
        public ChangeRestaurantContactDetailsCommand(RestaurantId restaurantId, string phone, string webSite, string imprint)
        {
            RestaurantId = restaurantId;
            Phone = phone;
            WebSite = webSite;
            Imprint = imprint;
        }

        public RestaurantId RestaurantId { get; }
        public string Phone { get; }
        public string WebSite { get; }
        public string Imprint { get; }
    }
}
