using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandler : IQueryHandler<GetRestaurantByIdQuery>
    {
        private readonly IRestaurantRepository restaurantRepository;

        public GetRestaurantByIdQueryHandler(IRestaurantRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<QueryResult> HandleAsync(GetRestaurantByIdQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return new UnauthorizedQueryResult();

            if (currentUser.Role < Role.RestaurantAdmin)
                return new ForbiddenQueryResult();

            return new SuccessQueryResult<Restaurant>(await restaurantRepository.FindByRestaurantIdAsync(query.RestaurantId));
        }
    }
}
