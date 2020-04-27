using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public interface IRestaurantRepository
    {
        Task<ICollection<Restaurant>> SearchAsync(string searchPhrase, CancellationToken cancellationToken = default);

        Task<Restaurant> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);

        Task<ICollection<Restaurant>> FindAllAsync(CancellationToken cancellationToken = default);

        Task StoreAsync(Restaurant restaurant, CancellationToken cancellationToken = default);

        Task RemoveAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);
    }
}
