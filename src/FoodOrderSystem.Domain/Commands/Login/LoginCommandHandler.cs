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
            if (string.IsNullOrWhiteSpace(command.Username) || string.IsNullOrWhiteSpace(command.Password))
                return FailureResult<UserViewModel>.Create(FailureResultCode.RequiredFieldEmpty);

            var user = await userRepository.FindByNameAsync(command.Username, cancellationToken);
            if (user == null)
                return FailureResult<UserViewModel>.Unauthorized();

            var validationResult = user.ValidatePassword(command.Password);
            if (validationResult.IsFailure)
                return FailureResult<UserViewModel>.Unauthorized();

            return SuccessResult<UserViewModel>.Create(UserViewModel.FromUser(user));
        }
    }
}
