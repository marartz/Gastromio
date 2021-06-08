using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.ChangeRestaurantContactInfo
{
    public class ChangeRestaurantContactInfoCommandHandler : ICommandHandler<ChangeRestaurantContactInfoCommand>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public ChangeRestaurantContactInfoCommandHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task HandleAsync(ChangeRestaurantContactInfoCommand command, User currentUser, CancellationToken cancellationToken = default)
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

            var contactInfo = new ContactInfo(command.Phone, command.Fax, command.WebSite, command.ResponsiblePerson,
                command.EmailAddress, command.Mobile, command.OrderNotificationByMobile);

            restaurant.ChangeContactInfo(contactInfo, currentUser.Id);

            await restaurantRepository.StoreAsync(restaurant, cancellationToken);
        }
    }
}
