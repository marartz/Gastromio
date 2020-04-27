using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler : IQueryHandler<GetAllRestaurantsQuery>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public GetAllRestaurantsQueryHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<QueryResult> HandleAsync(GetAllRestaurantsQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return new UnauthorizedQueryResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenQueryResult();

            return new SuccessQueryResult<ICollection<Restaurant>>(await restaurantRepository.FindAllAsync(cancellationToken));
        }
    }
}
