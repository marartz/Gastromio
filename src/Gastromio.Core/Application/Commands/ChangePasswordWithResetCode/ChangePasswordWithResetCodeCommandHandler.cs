using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.ChangePasswordWithResetCode
{
    public class ChangePasswordWithResetCodeCommandHandler : ICommandHandler<ChangePasswordWithResetCodeCommand>
    {
        private readonly IUserRepository userRepository;

        public ChangePasswordWithResetCodeCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task HandleAsync(ChangePasswordWithResetCodeCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var user = await userRepository.FindByUserIdAsync(command.UserId, cancellationToken);

            if (user == null)
                throw DomainException.CreateFrom(new PasswordResetCodeIsInvalidFailure());

            user.ChangePasswordWithResetCode(command.PasswordResetCode, command.Password);
            await userRepository.StoreAsync(user, cancellationToken);
        }
    }
}
