using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.AddRestaurant
{
    public class AddRestaurantCommandHandler : ICommandHandler<AddRestaurantCommand>
    {
        private readonly IRestaurantFactory restaurantFactory;
        private readonly IRestaurantRepository restaurantRepository;

        public AddRestaurantCommandHandler(IRestaurantFactory restaurantFactory, IRestaurantRepository restaurantRepository)
        {
            this.restaurantFactory = restaurantFactory;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<CommandResult> HandleAsync(AddRestaurantCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult();

            var restaurant = restaurantFactory.CreateWithName(command.Name);
            await restaurantRepository.StoreAsync(restaurant, cancellationToken);

            return new SuccessCommandResult<Restaurant>(restaurant);
        }
    }
}
