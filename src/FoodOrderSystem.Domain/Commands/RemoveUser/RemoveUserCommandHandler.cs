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

        public async Task<CommandResult<bool>> HandleAsync(RemoveUserCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult<bool>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult<bool>();

            if (command.UserId == currentUser.Id)
                return new FailureCommandResult<bool>();

            await userRepository.RemoveAsync(command.UserId, cancellationToken);

            return new SuccessCommandResult<bool>(true);
        }
    }
}
