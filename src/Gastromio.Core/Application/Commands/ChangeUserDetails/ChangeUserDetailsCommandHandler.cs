using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.ChangeUserDetails
{
    public class ChangeUserDetailsCommandHandler : ICommandHandler<ChangeUserDetailsCommand>
    {
        private readonly IUserRepository userRepository;

        public ChangeUserDetailsCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task HandleAsync(ChangeUserDetailsCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var user = await userRepository.FindByUserIdAsync(command.UserId, cancellationToken);
            if (user == null)
                throw DomainException.CreateFrom(new UserDoesNotExistFailure());

            user.ChangeDetails(command.Role, command.Email, currentUser.Id);

            await userRepository.StoreAsync(user, cancellationToken);
        }
    }
}
