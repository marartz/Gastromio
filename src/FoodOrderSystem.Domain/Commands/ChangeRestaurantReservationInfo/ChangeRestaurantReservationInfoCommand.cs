using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantReservationInfo
{
    public class ChangeRestaurantReservationInfoCommand : ICommand<bool>
    {
        public ChangeRestaurantReservationInfoCommand(RestaurantId restaurantId, ReservationInfo reservationInfo)
        {
            RestaurantId = restaurantId;
            ReservationInfo = reservationInfo;
        }

        public RestaurantId RestaurantId { get; }
    
        public ReservationInfo ReservationInfo { get; }
    }
}
