using FoodOrderSystem.Domain.Model.User;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.PaymentMethod;

namespace FoodOrderSystem.Domain.Model.Restaurant
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> SearchAsync(string searchPhrase, CancellationToken cancellationToken = default);

        Task<Restaurant> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Restaurant>> FindByCuisineIdAsync(CuisineId cuisineId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Restaurant>> FindByPaymentMethodIdAsync(PaymentMethodId paymentMethodId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Restaurant>> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Restaurant>> FindAllAsync(CancellationToken cancellationToken = default);

        Task StoreAsync(Restaurant restaurant, CancellationToken cancellationToken = default);

        Task RemoveAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);
    }
}
