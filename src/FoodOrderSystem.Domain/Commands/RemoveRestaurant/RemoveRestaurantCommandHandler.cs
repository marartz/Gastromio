using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.RemoveRestaurant
{
    public class RemoveRestaurantCommandHandler : ICommandHandler<RemoveRestaurantCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public RemoveRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<CommandResult> HandleAsync(RemoveRestaurantCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult();

            await restaurantRepository.RemoveAsync(command.RestaurantId, cancellationToken);

            return new SuccessCommandResult();
        }
    }
}
