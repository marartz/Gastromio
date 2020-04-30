using FoodOrderSystem.Domain.Model.User;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<QueryResult<TResult>> HandleAsync(TQuery query, User currentUser, CancellationToken cancellationToken = default);
    }
}
