﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands.AddUser
{
    public class AddUserCommandHandler : ICommandHandler<AddUserCommand, UserDTO>
    {
        private readonly IUserFactory userFactory;
        private readonly IUserRepository userRepository;

        public AddUserCommandHandler(IUserFactory userFactory, IUserRepository userRepository)
        {
            this.userFactory = userFactory;
            this.userRepository = userRepository;
        }

        public async Task<UserDTO> HandleAsync(AddUserCommand command, User currentUser, CancellationToken cancellationToken = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var user = await userRepository.FindByEmailAsync(command.Email, cancellationToken);
            if (user != null)
                throw DomainException.CreateFrom(new UserAlreadyExistsFailure());

            user = userFactory.Create(
                command.Role,
                command.Email,
                command.Password,
                true,
                currentUser.Id
            );

            await userRepository.StoreAsync(user, cancellationToken);

            return new UserDTO(user);
        }
    }
}
