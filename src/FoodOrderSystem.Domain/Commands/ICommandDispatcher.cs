using FoodOrderSystem.Domain.Model.User;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands
{
    public interface ICommandDispatcher
    {
        Task<CommandResult<TResult>> PostAsync<TCommand, TResult>(TCommand Command, User currentUser, CancellationToken cancellationToken = default) where TCommand : ICommand<TResult>;
    }
}
