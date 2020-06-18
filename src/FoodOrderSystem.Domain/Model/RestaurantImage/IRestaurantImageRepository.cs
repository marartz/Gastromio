using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Model.RestaurantImage
{
    public interface IRestaurantImageRepository
    {
        Task<RestaurantImage> FindByRestaurantImageIdAsync(RestaurantImageId restaurantImageId,
            CancellationToken cancellationToken = default);

        Task<RestaurantImage> FindByRestaurantIdAndTypeAsync(RestaurantId restaurantId, string type,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<RestaurantImage>> FindByRestaurantIdAsync(RestaurantId restaurantId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<string>> FindTypesByRestaurantIdAsync(RestaurantId restaurantId,
            CancellationToken cancellationToken = default);

        Task StoreAsync(RestaurantImage restaurantImage, CancellationToken cancellationToken = default);

        Task RemoveByRestaurantImageId(RestaurantImageId restaurantImageId, string type,
            CancellationToken cancellationToken = default);

        Task RemoveByRestaurantIdAndTypeAsync(RestaurantId restaurantId, string type,
            CancellationToken cancellationToken = default);

        Task RemoveByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);
    }
}