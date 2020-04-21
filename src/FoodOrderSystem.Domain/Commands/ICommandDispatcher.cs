using FoodOrderSystem.Domain.Model.User;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands
{
    public interface ICommandDispatcher
    {
        Task<CommandResult> PostAsync<TCommand>(TCommand Command, User currentUser, CancellationToken cancellationToken = default) where TCommand : ICommand;
    }
}
