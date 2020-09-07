using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Domain.Model.Order;
using FoodOrderSystem.Core.Domain.Model.Restaurant;

namespace FoodOrderSystem.Core.Application.Ports.Persistence
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);

        Task<IEnumerable<Order>> FindByPendingCustomerNotificationAsync(CancellationToken cancellationToken = default);

        Task<IEnumerable<Order>> FindByPendingRestaurantNotificationAsync(CancellationToken cancellationToken = default);

        Task<Order> FindByOrderIdAsync(OrderId orderId, CancellationToken cancellationToken = default);

        Task StoreAsync(Order order, CancellationToken cancellationToken = default);

        Task RemoveByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default);

        Task RemoveAsync(OrderId orderId, CancellationToken cancellationToken = default);
    }
}