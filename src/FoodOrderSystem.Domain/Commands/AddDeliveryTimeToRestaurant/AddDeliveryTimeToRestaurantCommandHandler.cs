using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.AddDeliveryTimeToRestaurant
{
    public class AddDeliveryTimeToRestaurantCommandHandler : ICommandHandler<AddDeliveryTimeToRestaurantCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public AddDeliveryTimeToRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<CommandResult> HandleAsync(AddDeliveryTimeToRestaurantCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult();

            if (currentUser.Role < Role.RestaurantAdmin)
                return new ForbiddenCommandResult();

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant != null)
                return new FailureCommandResult<string>("restaurant does not exist");

            restaurant.AddDeliveryTime(command.DayOfWeek, command.Start, command.End);

            return new SuccessCommandResult<Restaurant>(restaurant);
        }
    }
}
