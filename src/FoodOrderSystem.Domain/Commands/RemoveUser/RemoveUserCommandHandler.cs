using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.RemoveUser
{
    public class RemoveUserCommandHandler : ICommandHandler<RemoveUserCommand, bool>
    {
        private readonly IUserRepository userRepository;

        public RemoveUserCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<bool>> HandleAsync(RemoveUserCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return FailureResult<bool>.Unauthorized();

            if (currentUser.Role < Role.SystemAdmin)
                return FailureResult<bool>.Forbidden();

            if (command.UserId == currentUser.Id)
                return FailureResult<bool>.Create(FailureResultCode.CannotRemoveCurrentUser);

            await userRepository.RemoveAsync(command.UserId, cancellationToken);

            return SuccessResult<bool>.Create(true);
        }
    }
}
