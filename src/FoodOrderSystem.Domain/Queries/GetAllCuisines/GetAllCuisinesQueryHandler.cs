using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.User;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.GetAllCuisines
{
    public class GetAllCuisinesQueryHandler : IQueryHandler<GetAllCuisinesQuery>
    {
        private readonly ICuisineRepository cuisineRepository;

        public GetAllCuisinesQueryHandler(ICuisineRepository cuisineRepository)
        {
            this.cuisineRepository = cuisineRepository;
        }

        public async Task<QueryResult> HandleAsync(GetAllCuisinesQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                return new UnauthorizedQueryResult();

            if (currentUser.Role < Role.SystemAdmin)
                return new ForbiddenQueryResult();

            return new SuccessQueryResult<ICollection<Cuisine>>(await cuisineRepository.FindAllAsync(cancellationToken));
        }
    }
}
