using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Commands.ChangeDeviatingOpeningDayStatusOfRestaurant
{
    public class ChangeDeviatingOpeningDayStatusOfRestaurantCommand : ICommand<bool>
    {
        public ChangeDeviatingOpeningDayStatusOfRestaurantCommand(RestaurantId restaurantId, Date date,
            DeviatingOpeningDayStatus status)
        {
            RestaurantId = restaurantId;
            Date = date;
            Status = status;
        }
        
        public RestaurantId RestaurantId { get; }
        public Date Date { get; }
        public DeviatingOpeningDayStatus Status { get; }
    }
}