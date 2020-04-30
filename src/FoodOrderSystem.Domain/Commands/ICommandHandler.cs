using FoodOrderSystem.Domain.Model.User;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands
{
    public interface ICommandHandler<in TCommand, TResult> where TCommand : ICommand<TResult>
    {
        Task<CommandResult<TResult>> HandleAsync(TCommand command, User currentUser, CancellationToken cancellationToken = default);
    }
}
