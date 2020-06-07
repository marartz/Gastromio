using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.RemovePaymentMethod
{
    public class RemovePaymentMethodCommandHandler : ICommandHandler<RemovePaymentMethodCommand, bool>
    {
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IRestaurantRepository restaurantRepository;

        public RemovePaymentMethodCommandHandler(IPaymentMethodRepository paymentMethodRepository,
            IRestaurantRepository restaurantRepository)
        {
            this.paymentMethodRepository = paymentMethodRepository;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<Result<bool>> HandleAsync(RemovePaymentMethodCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<bool>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<bool>.Forbidden();

            var restaurants = await restaurantRepository.FindByPaymentMethodIdAsync(command.PaymentMethodId, cancellationToken);
            foreach (var restaurant in restaurants)
            {
                restaurant.RemovePaymentMethod(command.PaymentMethodId);
                await restaurantRepository.StoreAsync(restaurant, cancellationToken);
            }
            
            await paymentMethodRepository.RemoveAsync(command.PaymentMethodId, cancellationToken);

            return SuccessResult<bool>.Create(true);
        }
    }
}
