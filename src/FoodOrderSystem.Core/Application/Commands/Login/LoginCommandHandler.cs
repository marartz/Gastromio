using System;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Commands.Login
{
    public class LoginCommandHandler : ICommandHandler<LoginCommand, UserDTO>
    {
        private readonly IUserRepository userRepository;

        public LoginCommandHandler(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<Result<UserDTO>> HandleAsync(LoginCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
            
            if (string.IsNullOrWhiteSpace(command.Email))
                return FailureResult<UserDTO>.Create(FailureResultCode.RequiredFieldEmpty, "Email");

            if (string.IsNullOrWhiteSpace(command.Password))
                return FailureResult<UserDTO>.Create(FailureResultCode.RequiredFieldEmpty, "Password");

            var user = await userRepository.FindByEmailAsync(command.Email, cancellationToken);
            if (user == null)
                return FailureResult<UserDTO>.Unauthorized(FailureResultCode.WrongCredentials);

            var validationResult = user.ValidatePassword(command.Password);
            if (validationResult.IsFailure)
                return FailureResult<UserDTO>.Unauthorized(FailureResultCode.WrongCredentials);
            if (!validationResult.Value)
                return FailureResult<UserDTO>.Unauthorized(FailureResultCode.WrongCredentials);

            return SuccessResult<UserDTO>.Create(new UserDTO(user));
        }
    }
}
