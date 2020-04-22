using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Model.Cuisine
{
    public interface ICuisineRepository
    {
        Task<ICollection<Cuisine>> FindAllAsync(CancellationToken cancellationToken = default);

        Task<Cuisine> FindByNameAsync(string name, CancellationToken cancellationToken = default);

        Task<Cuisine> FindByCuisineIdAsync(CuisineId cuisineId, CancellationToken cancellationToken = default);

        Task StoreAsync(Cuisine cuisine, CancellationToken cancellationToken = default);

        Task RemoveAsync(CuisineId cuisineId, CancellationToken cancellationToken = default);
    }
}
