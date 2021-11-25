﻿using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Commands
{
    public interface ICommandDispatcher
    {
        Task PostAsync<TCommand>(TCommand command, UserId currentUserId, CancellationToken cancellationToken = default) where TCommand : ICommand;

        Task<TResult> PostAsync<TCommand, TResult>(TCommand command, UserId currentUserId, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>;
    }
}
