using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.ChangeUserDetails
{
    public class ChangeUserDetailsCommandHandler : ICommandHandler<ChangeUserDetailsCommand>
    {
        private readonly IUserRepository userRepository;

        public ChangeUserDetailsCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<CommandResult> HandleAsync(ChangeUserDetailsCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult();

            var user = await userRepository.FindByUserIdAsync(command.UserId, cancellationToken);
            if (user == null)
                return new FailureCommandResult<string>("user does not exist");

            user.ChangeDetails(command.Name, command.Role);

            await userRepository.StoreAsync(user, cancellationToken);

            return new SuccessCommandResult<User>(user);
        }
    }
}
