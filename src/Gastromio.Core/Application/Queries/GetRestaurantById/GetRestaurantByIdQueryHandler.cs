﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Ports.Persistence;
using Gastromio.Core.Common;
using Gastromio.Core.Domain.Failures;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;

namespace Gastromio.Core.Application.Queries.GetRestaurantById
{
    public class GetRestaurantByIdQueryHandler : IQueryHandler<GetRestaurantByIdQuery, RestaurantDTO>
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IRestaurantImageRepository restaurantImageRepository;
        private readonly ICuisineRepository cuisineRepository;
        private readonly IPaymentMethodRepository paymentMethodRepository;
        private readonly IUserRepository userRepository;

        public GetRestaurantByIdQueryHandler(
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

        public async Task<RestaurantDTO> HandleAsync(GetRestaurantByIdQuery query, User currentUser, CancellationToken cancellationToken = default)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var cuisines = (await cuisineRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id, en => new CuisineDTO(en));

            var paymentMethods = (await paymentMethodRepository.FindAllAsync(cancellationToken))
                .ToDictionary(en => en.Id, en => new PaymentMethodDTO(en));

            Restaurant restaurant;

            if (Guid.TryParse(query.RestaurantId, out var restaurantId))
            {
                restaurant =
                    await restaurantRepository.FindByRestaurantIdAsync(new RestaurantId(restaurantId),
                        cancellationToken);
            }
            else
            {
                restaurant =
                    (await restaurantRepository.FindByRestaurantNameAsync(query.RestaurantId, cancellationToken))
                    .FirstOrDefault();
            }

            if (restaurant == null)
                throw DomainException.CreateFrom(new RestaurantDoesNotExistFailure());

            if (query.OnlyActiveRestaurants && !restaurant.IsActive)
                throw DomainException.CreateFrom(new RestaurantDoesNotExistFailure());

            var userIds = restaurant.Administrators
                .Union(new[] {restaurant.CreatedBy, restaurant.UpdatedBy})
                .Distinct();

            var users = await userRepository.FindByUserIdsAsync(userIds, cancellationToken);

            var userDict = users != null
                ? users.ToDictionary(user => user.Id, user => new UserDTO(user))
                : new Dictionary<UserId, UserDTO>();

            var restaurantImageTypes =
                await restaurantImageRepository.FindTypesByRestaurantIdsAsync(new[] {restaurant.Id}, cancellationToken);

            return new RestaurantDTO(restaurant, cuisines, paymentMethods, userDict, restaurantImageTypes);
        }
    }
}
