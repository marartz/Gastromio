using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Model.User
{
    public interface IUserRepository
    {
        Task<ICollection<User>> FindAllAsync(CancellationToken cancellationToken = default);

        Task<ICollection<User>> SearchAsync(string searchPhrase, CancellationToken cancellationToken = default);

        Task<ICollection<User>> FindByRoleAsync(Role role, CancellationToken cancellationToken = default);

        Task<User> FindByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<User> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);

        Task StoreAsync(User user, CancellationToken cancellationToken = default);

        Task RemoveAsync(UserId userId, CancellationToken cancellationToken = default);
    }
}
