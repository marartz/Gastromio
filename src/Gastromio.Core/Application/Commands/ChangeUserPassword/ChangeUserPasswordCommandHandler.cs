using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.ChangeUserPassword
{
    public class ChangeUserPasswordCommandHandler : ICommandHandler<ChangeUserPasswordCommand>
    {
        private readonly IUserRepository userRepository;

        public ChangeUserPasswordCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task HandleAsync(ChangeUserPasswordCommand command, User currentUser, CancellationToken cancellationToken = default)
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

            user.ChangePassword(command.Password, true, currentUser.Id);

            await userRepository.StoreAsync(user, cancellationToken);
        }
    }
}
