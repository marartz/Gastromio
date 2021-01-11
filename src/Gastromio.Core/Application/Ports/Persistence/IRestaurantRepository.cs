using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Domain.Model.Cuisine;
using Gastromio.Core.Domain.Model.Order;
using Gastromio.Core.Domain.Model.PaymentMethod;
using Gastromio.Core.Domain.Model.Restaurant;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Application.Ports.Persistence
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> SearchAsync(string searchPhrase, OrderType? orderType, CuisineId cuisineId,
            DateTime? openingHour, bool? isActive, CancellationToken cancellationToken = default);

        Task<(long total, IEnumerable<Restaurant> items)> SearchPagedAsync(string searchPhrase, OrderType? orderType,
            CuisineId cuisineId, DateTime? openingHour, bool? isActive, int skip = 0, int take = -1, CancellationToken cancellationToken = default);

        Task<Restaurant> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Restaurant>> FindByRestaurantNameAsync(string restaurantName, CancellationToken cancellationToken = default);

        Task<Restaurant> FindByImportIdAsync(string importId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Restaurant>> FindByCuisineIdAsync(CuisineId cuisineId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Restaurant>> FindByPaymentMethodIdAsync(PaymentMethodId paymentMethodId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Restaurant>> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Restaurant>> FindAllAsync(CancellationToken cancellationToken = default);

        Task StoreAsync(Restaurant restaurant, CancellationToken cancellationToken = default);

        Task RemoveAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);
    }
}
