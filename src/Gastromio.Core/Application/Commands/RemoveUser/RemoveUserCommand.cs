﻿using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Application.Commands.RemoveUser
{
    public class RemoveUserCommand : ICommand<bool>
    {
        public RemoveUserCommand(UserId userId)
        {
            UserId = userId;
        }

        public UserId UserId { get; }
    }
}
