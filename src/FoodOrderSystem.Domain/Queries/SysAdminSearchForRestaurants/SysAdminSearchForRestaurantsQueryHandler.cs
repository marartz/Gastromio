using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.SysAdminSearchForRestaurants
{
    public class SysAdminSearchForRestaurantsQueryHandler : IQueryHandler<SysAdminSearchForRestaurantsQuery>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public SysAdminSearchForRestaurantsQueryHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<QueryResult> HandleAsync(SysAdminSearchForRestaurantsQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return new UnauthorizedQueryResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenQueryResult();

            return new SuccessQueryResult<ICollection<Restaurant>>(await restaurantRepository.SearchAsync(query.SearchPhrase, cancellationToken));
        }
    }
}
