using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.RemoveRestaurant
{
    public class RemoveRestaurantCommandHandler : ICommandHandler<RemoveRestaurantCommand, bool>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public RemoveRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<CommandResult<bool>> HandleAsync(RemoveRestaurantCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult<bool>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult<bool>();

            await restaurantRepository.RemoveAsync(command.RestaurantId, cancellationToken);

            return new SuccessCommandResult<bool>(true);
        }
    }
}
