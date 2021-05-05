using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.Login
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
                throw DomainException.CreateFrom(new LoginEmailRequiredFailure());

            if (string.IsNullOrWhiteSpace(command.Password))
                throw DomainException.CreateFrom(new LoginPasswordRequiredFailure());

            var user = await userRepository.FindByEmailAsync(command.Email, cancellationToken);
            if (user == null)
                throw DomainException.CreateFrom(new WrongCredentialsFailure());

            var valid = user.ValidatePassword(command.Password);
            if (!valid)
                throw DomainException.CreateFrom(new WrongCredentialsFailure());

            return SuccessResult<UserDTO>.Create(new UserDTO(user));
        }
    }
}
