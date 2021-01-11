using Gastromio.Core.Domain.Model.PaymentMethod;
using Gastromio.Core.Domain.Model.Restaurant;

namespace Gastromio.Core.Application.Commands.AddPaymentMethodToRestaurant
{
    public class AddPaymentMethodToRestaurantCommand : ICommand<bool>
    {
        public AddPaymentMethodToRestaurantCommand(RestaurantId restaurantId, PaymentMethodId paymentMethodId)
        {
            RestaurantId = restaurantId;
            PaymentMethodId = paymentMethodId;
        }

        public RestaurantId RestaurantId { get; }
        public PaymentMethodId PaymentMethodId { get; }
    }
}
