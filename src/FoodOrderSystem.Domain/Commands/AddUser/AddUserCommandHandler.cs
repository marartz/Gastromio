using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.AddUser
{
    public class AddUserCommandHandler : ICommandHandler<AddUserCommand, UserViewModel>
    {
        private readonly IUserFactory userFactory;
        private readonly IUserRepository userRepository;

        public AddUserCommandHandler(IUserFactory userFactory, IUserRepository userRepository)
        {
            this.userFactory = userFactory;
            this.userRepository = userRepository;
        }

        public async Task<CommandResult<UserViewModel>> HandleAsync(AddUserCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                return new UnauthorizedCommandResult<UserViewModel>();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenCommandResult<UserViewModel>();

            var user = await userRepository.FindByNameAsync(command.Name, cancellationToken);
            if (user != null)
                return new FailureCommandResult<UserViewModel>();

            user = userFactory.Create(command.Name, command.Role, command.Email, command.Password);

            await userRepository.StoreAsync(user, cancellationToken);

            return new SuccessCommandResult<UserViewModel>(UserViewModel.FromUser(user));
        }
    }
}
