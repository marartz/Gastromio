using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.ChangeRestaurantServiceInfo
{
    public class ChangeRestaurantServiceInfoCommandHandler : ICommandHandler<ChangeRestaurantServiceInfoCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public ChangeRestaurantServiceInfoCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task HandleAsync(ChangeRestaurantServiceInfoCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.RestaurantAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var restaurant = await restaurantRepository.FindByRestaurantIdAsync(command.RestaurantId, cancellationToken);
            if (restaurant == null)
                throw DomainException.CreateFrom(new RestaurantDoesNotExistFailure());

            if (currentUser.Role == Role.RestaurantAdmin && !restaurant.HasAdministrator(currentUser.Id))
                throw DomainException.CreateFrom(new ForbiddenFailure());

            restaurant.ChangePickupInfo(command.PickupInfo, currentUser.Id);
            restaurant.ChangeDeliveryInfo(command.DeliveryInfo, currentUser.Id);
            restaurant.ChangeReservationInfo(command.ReservationInfo, currentUser.Id);
            restaurant.ChangeHygienicHandling(command.HygienicHandling, currentUser.Id);

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);
        }
    }
}
