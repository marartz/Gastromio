using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.ChangeRestaurantName
{
    public class ChangeRestaurantNameCommandHandler : ICommandHandler<ChangeRestaurantNameCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public ChangeRestaurantNameCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task HandleAsync(ChangeRestaurantNameCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                throw DomainException.CreateFrom(new RestaurantDoesNotExistFailure());

            if (currentUser.Role == Role.RestaurantAdmin && !restaurant.HasAdministrator(currentUser.Id))
                throw DomainException.CreateFrom(new ForbiddenFailure());

            if (string.Equals(restaurant.Name?.Trim(), command.Name?.Trim()))
                return;

            var existingRestaurants =
                await restaurantRepository.FindByRestaurantNameAsync(command.Name, cancellationToken);
            if (existingRestaurants.Any())
                throw DomainException.CreateFrom(new RestaurantAlreadyExistsFailure());

            restaurant.ChangeName(command.Name, currentUser.Id);

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);
        }
    }
}
