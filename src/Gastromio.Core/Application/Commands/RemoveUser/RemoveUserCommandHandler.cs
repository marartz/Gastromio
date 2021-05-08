using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.RemoveUser
{
    public class RemoveUserCommandHandler : ICommandHandler<RemoveUserCommand>
    {
        private readonly IUserRepository userRepository;
        private readonly IRestaurantRepository restaurantRepository;

        public RemoveUserCommandHandler(IUserRepository userRepository, IRestaurantRepository restaurantRepository)
        {
            this.userRepository = userRepository;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task HandleAsync(RemoveUserCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            if (command.UserId == currentUser.Id)
                throw DomainException.CreateFrom(new CannotRemoveCurrentUserFailure());

            var restaurants = await restaurantRepository.FindByUserIdAsync(command.UserId, cancellationToken);
            var restaurantList = restaurants.ToList();
            if (restaurantList.Any())
            {
                throw DomainException.CreateFrom(
                    new UserIsRestaurantAdminFailure(restaurantList.Select(en => en.Name)));
            }

            await userRepository.RemoveAsync(command.UserId, cancellationToken);
        }
    }
}
