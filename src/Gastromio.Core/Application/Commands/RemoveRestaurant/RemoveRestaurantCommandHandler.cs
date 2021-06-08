using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.RemoveRestaurant
{
    public class RemoveRestaurantCommandHandler : ICommandHandler<RemoveRestaurantCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IRestaurantImageRepository restaurantImageRepository;

        public RemoveRestaurantCommandHandler(
            IRestaurantRepository restaurantRepository,
            IRestaurantImageRepository restaurantImageRepository
        )
        {
            this.restaurantRepository = restaurantRepository;
            this.restaurantImageRepository = restaurantImageRepository;
        }

        public async Task HandleAsync(RemoveRestaurantCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            await restaurantImageRepository.RemoveByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            await restaurantRepository.RemoveAsync(command.RestaurantId, cancellationToken);
        }
    }
}
