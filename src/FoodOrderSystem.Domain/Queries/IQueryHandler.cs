using FoodOrderSystem.Domain.Model.User;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries
{
    public interface IQueryHandler<in TQuery> where TQuery : IQuery
    {
        Task<QueryResult> HandleAsync(TQuery query, User currentUser, CancellationToken cancellationToken = default);
    }
}
