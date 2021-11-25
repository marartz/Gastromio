using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.RemoveCuisine
{
    public class RemoveCuisineCommandHandler : ICommandHandler<RemoveCuisineCommand>
    {
        private readonly ICuisineRepository cuisineRepository;
        private readonly IRestaurantRepository restaurantRepository;

        public RemoveCuisineCommandHandler(ICuisineRepository cuisineRepository, IRestaurantRepository restaurantRepository)
        {
            this.cuisineRepository = cuisineRepository;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task HandleAsync(RemoveCuisineCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var restaurants = await restaurantRepository.FindByCuisineIdAsync(command.CuisineId, cancellationToken);
            foreach (var restaurant in restaurants)
            {
                restaurant.RemoveCuisine(command.CuisineId, currentUser.Id);
                await restaurantRepository.StoreAsync(restaurant, cancellationToken);
            }

            await cuisineRepository.RemoveAsync(command.CuisineId, cancellationToken);
        }
    }
}
