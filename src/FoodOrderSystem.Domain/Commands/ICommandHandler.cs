using FoodOrderSystem.Domain.Model.User;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<CommandResult> HandleAsync(TCommand command, User currentUser, CancellationToken cancellationToken = default);
    }
}
