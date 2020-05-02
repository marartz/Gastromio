using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, UserViewModel>
    {
        private readonly IUserRepository userRepository;

        public LoginCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<UserViewModel>> HandleAsync(LoginCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrWhiteSpace(command.Username))
                throw new InvalidOperationException("username is null or whitespace");
            if (string.IsNullOrWhiteSpace(command.Password))
                throw new InvalidOperationException("password is null or whitespace");

            var user = await userRepository.FindByNameAsync(command.Username, cancellationToken);
            if (user == null)
                return FailureResult<UserViewModel>.Unauthorized();

            if (!user.ValidatePassword(command.Password))
                return FailureResult<UserViewModel>.Unauthorized();

            return SuccessResult<UserViewModel>.Create(UserViewModel.FromUser(user));
        }
    }
}
