using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.EnableSupportForRestaurant
{
    public class EnableSupportForRestaurantCommandHandler : ICommandHandler<EnableSupportForRestaurantCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public EnableSupportForRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task HandleAsync(EnableSupportForRestaurantCommand command, User currentUser,
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

            restaurant.EnableSupport(currentUser.Id);

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);
        }
    }
}
