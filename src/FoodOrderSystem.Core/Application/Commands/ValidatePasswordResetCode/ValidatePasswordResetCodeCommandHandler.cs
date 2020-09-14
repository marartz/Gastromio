using System;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Commands.ValidatePasswordResetCode
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

            return user != null
                ? user.ValidatePasswordResetCode(command.PasswordResetCode)
                : FailureResult<bool>.Create(FailureResultCode.PasswordResetCodeIsInvalid);
        }
    }
}