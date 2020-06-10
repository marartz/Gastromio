using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantServiceInfo
{
    public class ChangeRestaurantServiceInfoCommandHandler : ICommandHandler<ChangeRestaurantServiceInfoCommand, bool>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public ChangeRestaurantServiceInfoCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<Result<bool>> HandleAsync(ChangeRestaurantServiceInfoCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<bool>.Unauthorized();

            if (currentUser.Role < Role.RestaurantAdmin)
                return FailureResult<bool>.Forbidden();

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                return FailureResult<bool>.Create(FailureResultCode.RestaurantDoesNotExist);

            if (currentUser.Role == Role.RestaurantAdmin && !restaurant.HasAdministrator(currentUser.Id))
                return FailureResult<bool>.Forbidden();

            var result = restaurant.ChangePickupInfo(command.PickupInfo);
            if (result.IsFailure)
                return result;

            result = restaurant.ChangeDeliveryInfo(command.DeliveryInfo);
            if (result.IsFailure)
                return result;

            result = restaurant.ChangeReservationInfo(command.ReservationInfo);
            if (result.IsFailure)
                return result;

            result = restaurant.ChangeHygienicHandling(command.HygienicHandling);
            if (result.IsFailure)
                return result;

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            return result;
        }
    }
}
