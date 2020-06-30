using System;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;

namespace FoodOrderSystem.Domain.Commands.DisableSupportForRestaurant
{
    public class DisableSupportForRestaurantCommandHandler : ICommandHandler<DisableSupportForRestaurantCommand, bool>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public DisableSupportForRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }
        
        public async Task<Result<bool>> HandleAsync(DisableSupportForRestaurantCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<bool>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<bool>.Forbidden();

            var restaurant =
                await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantDoesNotExist);

            var tempResult = restaurant.DisableSupport(currentUser.Id);
            if (tempResult.IsFailure)
                return tempResult;

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);
            
            return SuccessResult<bool>.Create(true);
        }
    }
}