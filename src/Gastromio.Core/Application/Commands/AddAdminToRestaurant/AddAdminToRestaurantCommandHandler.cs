using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.AddAdminToRestaurant
{
    public class AddAdminToRestaurantCommandHandler : ICommandHandler<AddAdminToRestaurantCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public AddAdminToRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task HandleAsync(AddAdminToRestaurantCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var restaurant =
                await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                throw DomainException.CreateFrom(new RestaurantDoesNotExistFailure());

            restaurant.AddAdministrator(command.UserId, currentUser.Id);

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);
        }
    }
}
