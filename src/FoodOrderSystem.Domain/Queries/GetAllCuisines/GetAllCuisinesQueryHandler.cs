using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Domain.Queries.GetAllCuisines
{
    public class GetAllCuisinesQueryHandler : IQueryHandler<GetAllCuisinesQuery, ICollection<CuisineViewModel>>
    {
        private readonly ICuisineRepository cuisineRepository;

        public GetAllCuisinesQueryHandler(ICuisineRepository cuisineRepository)
        {
            this.cuisineRepository = cuisineRepository;
        }

        public async Task<QueryResult<ICollection<CuisineViewModel>>> HandleAsync(GetAllCuisinesQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var cuisines = await cuisineRepository.FindAllAsync(cancellationToken);

            return new SuccessQueryResult<ICollection<CuisineViewModel>>(cuisines.Select(CuisineViewModel.FromCuisine).ToList());
        }
    }
}
