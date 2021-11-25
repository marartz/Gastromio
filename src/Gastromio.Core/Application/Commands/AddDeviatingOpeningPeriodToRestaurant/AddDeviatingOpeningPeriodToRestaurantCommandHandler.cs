using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.AddDeviatingOpeningPeriodToRestaurant
{
    public class AddDeviatingOpeningPeriodToRestaurantCommandHandler : ICommandHandler<AddDeviatingOpeningPeriodToRestaurantCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public AddDeviatingOpeningPeriodToRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task HandleAsync(AddDeviatingOpeningPeriodToRestaurantCommand command,
            User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.RestaurantAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var restaurant =
                await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                throw DomainException.CreateFrom(new RestaurantDoesNotExistFailure());

            if (currentUser.Role == Role.RestaurantAdmin && !restaurant.HasAdministrator(currentUser.Id))
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var openingPeriod = new OpeningPeriod(command.Start, command.End);

            restaurant.AddDeviatingOpeningPeriod(command.Date, openingPeriod, currentUser.Id);
            await restaurantRepository.StoreAsync(restaurant, cancellationToken);
        }
    }
}
