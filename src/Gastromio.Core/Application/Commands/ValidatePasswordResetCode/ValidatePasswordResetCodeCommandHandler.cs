using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.ValidatePasswordResetCode
{
    public class ValidatePasswordResetCodeCommandHandler : ICommandHandler<ValidatePasswordResetCodeCommand>
    {
        private readonly IUserRepository userRepository;

        public ValidatePasswordResetCodeCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task HandleAsync(ValidatePasswordResetCodeCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var user = await userRepository.FindByUserIdAsync(command.UserId, cancellationToken);
            if (user == null)
                throw DomainException.CreateFrom(new PasswordResetCodeIsInvalidFailure());

            if (!user.ValidatePasswordResetCode(command.PasswordResetCode))
                throw DomainException.CreateFrom(new PasswordResetCodeIsInvalidFailure());
        }
    }
}
