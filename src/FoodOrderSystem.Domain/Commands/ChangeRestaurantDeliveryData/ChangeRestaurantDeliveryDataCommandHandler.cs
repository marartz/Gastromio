using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantDeliveryData
{
    public class ChangeRestaurantDeliveryDataCommandHandler : ICommandHandler<ChangeRestaurantDeliveryDataCommand, bool>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public ChangeRestaurantDeliveryDataCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<CommandResult<bool>> HandleAsync(ChangeRestaurantDeliveryDataCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult<bool>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult<bool>();

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                return new FailureCommandResult<bool>();

            restaurant.ChangeDeliveryData(command.MinimumOrderValue, command.DeliveryCosts);

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            return new SuccessCommandResult<bool>(true);
        }
    }
}
