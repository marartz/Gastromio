using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public interface IRestaurantRepository
    {
        Task<Restaurant> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);

        Task<ICollection<Restaurant>> FindAllAsync(CancellationToken cancellationToken = default);
    }
}
