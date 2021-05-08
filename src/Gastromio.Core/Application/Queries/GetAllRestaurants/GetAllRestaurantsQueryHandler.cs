using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler : IQueryHandler<GetAllRestaurantsQuery, ICollection<RestaurantDTO>>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IRestaurantImageRepository restaurantImageRepository;
        private readonly ICuisineRepository cuisineRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IUserRepository userRepository;

        public GetAllRestaurantsQueryHandler(
            IRestaurantRepository restaurantRepository,
            IRestaurantImageRepository restaurantImageRepository,
            ICuisineRepository cuisineRepository,
            IPaymentMethodRepository paymentMethodRepository,
            IUserRepository userRepository
        )
        {
            this.restaurantRepository = restaurantRepository;
            this.restaurantImageRepository = restaurantImageRepository;
            this.cuisineRepository = cuisineRepository;
            this.paymentMethodRepository = paymentMethodRepository;
            this.userRepository = userRepository;
        }

        public async Task<ICollection<RestaurantDTO>> HandleAsync(GetAllRestaurantsQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (currentUser == null)
                throw DomainException.CreateFrom(new SessionExpiredFailure());

            if (currentUser.Role < Role.SystemAdmin)
                throw DomainException.CreateFrom(new ForbiddenFailure());

            var cuisines = (await cuisineRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id, en => new CuisineDTO(en));

            var paymentMethods = (await paymentMethodRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id, en => new PaymentMethodDTO(en));

            var restaurants = (await restaurantRepository.FindAllAsync(cancellationToken)).ToList();

            var userIds = restaurants
                .SelectMany(restaurant =>
                    restaurant.Administrators.Union(new[] {restaurant.CreatedBy, restaurant.UpdatedBy}))
                .Distinct();

            var users = await userRepository.FindByUserIdsAsync(userIds, cancellationToken);

            var userDict = users != null
                ? users.ToDictionary(user => user.Id, user => new UserDTO(user))
                : new Dictionary<UserId, UserDTO>();

            var restaurantImageTypes =
                await restaurantImageRepository.FindTypesByRestaurantIdsAsync(
                    restaurants.Select(restaurant => restaurant.Id), cancellationToken);

            return restaurants
                .Select(en => new RestaurantDTO(en, cuisines, paymentMethods, userDict, restaurantImageTypes)).ToList();
        }
    }
}
