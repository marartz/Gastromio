using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.AddDeliveryTimeToRestaurant
{
    public class AddDeliveryTimeToRestaurantCommandHandler : ICommandHandler<AddDeliveryTimeToRestaurantCommand, bool>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public AddDeliveryTimeToRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<CommandResult<bool>> HandleAsync(AddDeliveryTimeToRestaurantCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult<bool>();

            if (currentUser.Role < Role.RestaurantAdmin)
                return new ForbiddenCommandResult<bool>();

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                return new FailureCommandResult<bool>();

            restaurant.AddDeliveryTime(command.DayOfWeek, command.Start, command.End);

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            return new SuccessCommandResult<bool>(true);
        }
    }
}
