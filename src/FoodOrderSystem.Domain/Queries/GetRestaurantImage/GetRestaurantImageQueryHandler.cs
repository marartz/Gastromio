using System;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.RestaurantImage;
using FoodOrderSystem.Domain.Model.User;

namespace FoodOrderSystem.Domain.Queries.GetRestaurantImage
{
    public class GetRestaurantImageQueryHandler : IQueryHandler<GetRestaurantImageQuery, byte[]>
    {
        private readonly IRestaurantImageRepository restaurantImageRepository;

        public GetRestaurantImageQueryHandler(IRestaurantImageRepository restaurantImageRepository)
        {
            this.restaurantImageRepository = restaurantImageRepository;
        }

        public async Task<Result<byte[]>> HandleAsync(GetRestaurantImageQuery query, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var restaurantImage =
                await restaurantImageRepository.FindByRestaurantIdAndTypeAsync(query.RestaurantId, query.Type,
                    cancellationToken);

            if (restaurantImage?.Data == null || restaurantImage.Data.Length == 0)
                return FailureResult<byte[]>.Create(FailureResultCode.RestaurantImageNotValid);
            
            return SuccessResult<byte[]>.Create(restaurantImage.Data);
        }
    }
}