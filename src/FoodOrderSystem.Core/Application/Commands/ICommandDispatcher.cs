using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Commands
{
    public interface ICommandDispatcher
    {
        Task<Result<TResult>> PostAsync<TCommand, TResult>(TCommand command, UserId currentUserId, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>;
    }
}
