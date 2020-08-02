using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Queries.GetAllCuisines
{
    public class GetAllCuisinesQueryHandler : IQueryHandler<GetAllCuisinesQuery, ICollection<CuisineDTO>>
    {
        private readonly ICuisineRepository cuisineRepository;

        public GetAllCuisinesQueryHandler(ICuisineRepository cuisineRepository)
        {
            this.cuisineRepository = cuisineRepository;
        }

        public async Task<Result<ICollection<CuisineDTO>>> HandleAsync(GetAllCuisinesQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var cuisines = await cuisineRepository.FindAllAsync(cancellationToken);

            return SuccessResult<ICollection<CuisineDTO>>.Create(cuisines.Select(cuisine => new CuisineDTO(cuisine)).ToList());
        }
    }
}
