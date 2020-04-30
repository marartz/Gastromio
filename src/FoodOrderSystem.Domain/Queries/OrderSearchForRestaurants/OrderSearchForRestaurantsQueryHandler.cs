using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.OrderSearchForRestaurants
{
    public class OrderSearchForRestaurantsQueryHandler : IQueryHandler<OrderSearchForRestaurantsQuery>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public OrderSearchForRestaurantsQueryHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<QueryResult> HandleAsync(OrderSearchForRestaurantsQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            return new SuccessQueryResult<ICollection<Restaurant>>(await restaurantRepository.SearchAsync(query.SearchPhrase, cancellationToken));
        }
    }
}
