using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Domain.Model.Cuisine;

namespace FoodOrderSystem.Core.Application.Ports.Persistence
{
    public interface ICuisineRepository
    {
        Task<IEnumerable<Cuisine>> FindAllAsync(CancellationToken cancellationToken = default);

        Task<Cuisine> FindByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<Cuisine> FindByCuisineIdAsync(CuisineId cuisineId, CancellationToken cancellationToken = default);

        Task StoreAsync(Cuisine cuisine, CancellationToken cancellationToken = default);

        Task RemoveAsync(CuisineId cuisineId, CancellationToken cancellationToken = default);
    }
}
