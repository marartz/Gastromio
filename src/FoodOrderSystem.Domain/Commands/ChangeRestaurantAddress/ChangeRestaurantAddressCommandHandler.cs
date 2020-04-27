using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantAddress
{
    public class ChangeRestaurantAddressCommandHandler : ICommandHandler<ChangeRestaurantAddressCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public ChangeRestaurantAddressCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<CommandResult> HandleAsync(ChangeRestaurantAddressCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult();

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                return new FailureCommandResult<string>("user does not exist");

            restaurant.ChangeAddress(command.Address);

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            return new SuccessCommandResult<Restaurant>(restaurant);
        }
    }
}
