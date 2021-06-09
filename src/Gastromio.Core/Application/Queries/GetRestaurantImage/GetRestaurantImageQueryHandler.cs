using System;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.RestaurantImages;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Queries.GetRestaurantImage
{
    public class GetRestaurantImageQueryHandler : IQueryHandler<GetRestaurantImageQuery, RestaurantImage>
    {
        private readonly IRestaurantImageRepository restaurantImageRepository;

        public GetRestaurantImageQueryHandler(IRestaurantImageRepository restaurantImageRepository)
        {
            this.restaurantImageRepository = restaurantImageRepository;
        }

        public async Task<RestaurantImage> HandleAsync(GetRestaurantImageQuery query, User currentUser,
            CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var restaurantImage =
                await restaurantImageRepository.FindByRestaurantIdAndTypeAsync(query.RestaurantId, query.Type,
                    cancellationToken);

            if (restaurantImage?.Data == null || restaurantImage.Data.Length == 0)
                throw DomainException.CreateFrom(new RestaurantImageNotValidFailure());

            return restaurantImage;
        }
    }
}
