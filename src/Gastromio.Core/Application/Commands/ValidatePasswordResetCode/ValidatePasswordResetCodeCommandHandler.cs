using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.ValidatePasswordResetCode
{
    public class ValidatePasswordResetCodeCommandHandler : ICommandHandler<ValidatePasswordResetCodeCommand, bool>
    {
        private readonly IUserRepository userRepository;

        public ValidatePasswordResetCodeCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<bool>> HandleAsync(ValidatePasswordResetCodeCommand command, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            var user = await userRepository.FindByUserIdAsync(command.UserId, cancellationToken);
            if (user == null)
                return FailureResult<bool>.Create(new PasswordResetCodeIsInvalidFailure());

            return SuccessResult<bool>.Create(user.ValidatePasswordResetCode(command.PasswordResetCode));
        }
    }
}
