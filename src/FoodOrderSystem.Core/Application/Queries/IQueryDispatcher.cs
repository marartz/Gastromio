using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Queries
{
    public interface IQueryDispatcher
    {
        Task<Result<TResult>> PostAsync<TQuery, TResult>(TQuery query, UserId currentUserId, CancellationToken cancellationToken = default) where TQuery : IQuery<TResult>;
    }
}
