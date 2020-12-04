using System;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Commands.DeactivateRestaurant
{
    public class DeactivateRestaurantCommandHandler : ICommandHandler<DeactivateRestaurantCommand, bool>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public DeactivateRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<Result<bool>> HandleAsync(DeactivateRestaurantCommand command, User currentUser, CancellationToken cancellationToken = default)
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

            if (currentUser.Role == Role.RestaurantAdmin && !restaurant.HasAdministrator(currentUser.Id))
                return FailureResult<bool>.Forbidden();

            var tempResult = restaurant.Deactivate(currentUser.Id);
            if (tempResult.IsFailure)
                return tempResult;

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);
            
            return SuccessResult<bool>.Create(true);
        }
        
    }
}