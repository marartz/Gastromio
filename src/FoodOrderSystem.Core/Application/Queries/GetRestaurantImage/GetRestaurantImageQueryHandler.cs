using System;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Core.Application.Ports.Persistence;
using FoodOrderSystem.Core.Common;
using FoodOrderSystem.Core.Domain.Model.RestaurantImage;
using FoodOrderSystem.Core.Domain.Model.User;

namespace FoodOrderSystem.Core.Application.Queries.GetRestaurantImage
{
    public class GetRestaurantImageQueryHandler : IQueryHandler<GetRestaurantImageQuery, RestaurantImage>
    {
        private readonly IRestaurantImageRepository restaurantImageRepository;

        public GetRestaurantImageQueryHandler(IRestaurantImageRepository restaurantImageRepository)
        {
            this.restaurantImageRepository = restaurantImageRepository;
        }

        public async Task<Result<RestaurantImage>> HandleAsync(GetRestaurantImageQuery query, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var restaurantImage =
                await restaurantImageRepository.FindByRestaurantIdAndTypeAsync(query.RestaurantId, query.Type,
                    cancellationToken);

            if (restaurantImage?.Data == null || restaurantImage.Data.Length == 0)
                return FailureResult<RestaurantImage>.Create(FailureResultCode.RestaurantImageNotValid);
            
            return SuccessResult<RestaurantImage>.Create(restaurantImage);
        }
    }
}